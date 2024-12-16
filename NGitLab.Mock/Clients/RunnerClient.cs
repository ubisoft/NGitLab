using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class RunnerClient : ClientBase, IRunnerClient
{
    public IEnumerable<Models.Runner> Accessible
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return GetOwnedRunners().Select(r => r.ToClientRunner(Context.User));
            }
        }
    }

    public IEnumerable<Models.Runner> All
    {
        get
        {
            if (!Context.User.IsAdmin)
            {
                throw new GitLabForbiddenException();
            }

            using (Context.BeginOperationScope())
            {
                var runners = Server.AllGroups.SelectMany(g => g.RegisteredRunners).Concat(Server.AllProjects.SelectMany(p => p.RegisteredRunners));
                var clientRunners = runners.Select(r => r.ToClientRunner(Context.User)).ToList();
                return clientRunners;
            }
        }
    }

    public RunnerClient(ClientContext context)
        : base(context)
    {
    }

    public Models.Runner this[long id]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var runner = Accessible.FirstOrDefault(r => r.Id == id) ?? throw new GitLabNotFoundException();
                return runner;
            }
        }
    }

    public void Delete(Models.Runner runner) => Delete(runner.Id);

    public void Delete(long runnerId)
    {
        using (Context.BeginOperationScope())
        {
            var projects = Server.AllProjects.Where(p => p.EnabledRunners.Any(r => r.Id == runnerId));
            if (projects.Any())
            {
                if (projects.Skip(1).Any())
                {
                    throw new GitLabBadRequestException("Runner is enabled in multiple projects");
                }
                var project = GetProject(projects.Single().Id, ProjectPermission.Edit);
                project.RemoveRunner(runnerId);
                return;
            }

            var groups = Server.AllGroups.Where(g => g.RegisteredRunners.Any(r => r.Id == runnerId));
            if (groups.Any())
            {
                var group = GetGroup(groups.Single().Id, GroupPermission.Edit);
                group.RemoveRunner(runnerId);
                return;
            }

            throw new GitLabNotFoundException("Runner is not found in any project or group");
        }
    }

    public Models.Runner Update(long runnerId, RunnerUpdate runnerUpdate)
    {
        using (Context.BeginOperationScope())
        {
            var runner = this[runnerId] ?? throw new GitLabNotFoundException();
            var runnerOnServer = GetServerRunner(runnerId);

            runnerOnServer.Paused = runnerUpdate.Paused ?? runnerOnServer.Paused;
            runnerOnServer.TagList = runnerUpdate.TagList ?? runnerOnServer.TagList;
            runnerOnServer.Description = !string.IsNullOrEmpty(runnerUpdate.Description) ? runnerUpdate.Description : runnerOnServer.Description;
            runnerOnServer.Locked = runnerUpdate.Locked ?? runnerOnServer.Locked;
            runnerOnServer.RunUntagged = runnerUpdate.RunUntagged ?? runnerOnServer.RunUntagged;

            return runner;
        }
    }

    public IEnumerable<Models.Runner> OfGroup(long groupId)
    {
        using (Context.BeginOperationScope())
        {
            var runners = GetGroup(groupId, GroupPermission.Edit).RegisteredRunners;
            return runners.Select(r => this[r.Id]).ToList();
        }
    }

    public GitLabCollectionResponse<Models.Runner> OfGroupAsync(long groupId)
    {
        return GitLabCollectionResponse.Create(OfGroup(groupId));
    }

    public IEnumerable<Models.Runner> OfProject(long projectId)
    {
        using (Context.BeginOperationScope())
        {
            var runnerRefs = GetProject(projectId, ProjectPermission.Edit).EnabledRunners;
            return runnerRefs.Select(r => this[r.Id]).ToList();
        }
    }

    public GitLabCollectionResponse<Models.Runner> OfProjectAsync(long projectId)
    {
        return GitLabCollectionResponse.Create(OfProject(projectId));
    }

    public IEnumerable<Models.Job> GetJobs(long runnerId, JobStatus? status = null)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use OfProject() or OfGroup() instead")]
    IEnumerable<Models.Runner> IRunnerClient.GetAvailableRunners(long projectId)
    {
        return OfProject(projectId);
    }

    public IEnumerable<Models.Runner> GetAllRunnersWithScope(RunnerScope scope)
    {
        throw new NotImplementedException();
    }

    public Models.Runner EnableRunner(long projectId, RunnerId runnerId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var runner = GetServerRunner(runnerId.Id);

            var runnerReference = new RunnerRef(runner);

            if (project.EnabledRunners.Contains(runnerReference))
            {
                throw new GitLabBadRequestException("Runner has already been taken");
            }

            project.EnabledRunners.Add(runnerReference);
            return runner.ToClientRunner(Context.User);
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Runner> EnableRunnerAsync(long projectId, RunnerId runnerId, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return EnableRunner(projectId, runnerId);
    }

    public void DisableRunner(long projectId, RunnerId runnerId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var runner = GetServerRunner(runnerId.Id);

            if (project.EnabledRunners.All(r => r.Id != runnerId.Id))
            {
                throw new GitLabNotFoundException();
            }

            var runnerReference = new RunnerRef(runner);
            project.EnabledRunners.Remove(runnerReference);
        }
    }

    private IEnumerable<Runner> GetOwnedRunners()
    {
        var groups = Server.AllGroups.Where(group => group.CanUserEditGroup(Context.User));
        var projects = Server.AllProjects.Where(project => project.CanUserEditProject(Context.User));
        var runners = groups.SelectMany(g => g.RegisteredRunners).Concat(projects.SelectMany(p => p.RegisteredRunners));
        return runners;
    }

    private Runner GetServerRunner(long id)
    {
        return GetOwnedRunners().FirstOrDefault(runner => runner.Id == id) ?? throw new GitLabNotFoundException();
    }

    public Models.Runner Register(RunnerRegister request)
    {
        using (Context.BeginOperationScope())
        {
            var project = Server.AllProjects.SingleOrDefault(p => string.Equals(p.RunnersToken, request.Token, StringComparison.Ordinal));
            if (project != null)
            {
                var runner = project.AddRunner(null, request.Description, request.IsActive() ?? true, request.Locked ?? true, false, request.RunUntagged ?? false);
                return runner.ToClientRunner(Context.User);
            }

            var group = Server.AllGroups.SingleOrDefault(g => string.Equals(g.RunnersToken, request.Token, StringComparison.Ordinal));
            if (group != null)
            {
                var runner = group.AddRunner(request.Description, paused: request.Paused ?? false, tagList: request.TagList, runUntagged: request.RunUntagged ?? false, locked: request.Locked ?? true);
                return runner.ToClientRunner(Context.User);
            }

            throw new GitLabNotFoundException("Invalid token: no matching project or group found.");
        }
    }
}
