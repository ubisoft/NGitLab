using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal class RunnerClient : ClientBase, IRunnerClient
    {
        public IEnumerable<Models.Runner> Accessible => GetOwnedRunners().Select(r => r.ToClientRunner());

        public IEnumerable<Models.Runner> All
        {
            get
            {
                if (!Context.User.IsAdmin)
                {
                    throw new GitLabForbiddenException();
                }

                var runners = Server.AllProjects.SelectMany(p => p.RegisteredRunners);
                var clientRunners = runners.Select(r => r.ToClientRunner());
                return clientRunners;
            }
        }

        public RunnerClient(ClientContext context)
            : base(context)
        {
        }

        public Models.Runner this[int id]
        {
            get
            {
                var runner = Accessible.FirstOrDefault(r => r.Id == id) ?? throw new GitLabNotFoundException();
                return runner;
            }
        }

        public void Delete(Models.Runner runner)
        {
            throw new NotImplementedException();
        }

        public void Delete(int runnerId)
        {
            throw new NotImplementedException();
        }

        public Models.Runner Update(int runnerId, RunnerUpdate runnerUpdate)
        {
            var runner = this[runnerId] ?? throw new GitLabNotFoundException();
            var runnerOnServer = GetServerRunner(runnerId);

            runnerOnServer.Active = runnerUpdate.Active ?? runnerOnServer.Active;
            runnerOnServer.TagList = runnerUpdate.TagList ?? runnerOnServer.TagList;
            runnerOnServer.Description = !string.IsNullOrEmpty(runnerUpdate.Description) ? runnerUpdate.Description : runnerOnServer.Description;
            runnerOnServer.Locked = runnerUpdate.Locked ?? runnerOnServer.Locked;
            runnerOnServer.RunUntagged = runnerUpdate.RunUntagged ?? runnerOnServer.RunUntagged;

            return runner;
        }

        public IEnumerable<Models.Runner> OfProject(int projectId)
        {
            var runnerRefs = GetProject(projectId, ProjectPermission.Edit).EnabledRunners;
            return runnerRefs.Select(r => this[r.Id]);
        }

        public IEnumerable<Models.Job> GetJobs(int runnerId, JobScope jobScope)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Job> GetJobs(int runnerId, JobStatus? status = null)
        {
            throw new NotImplementedException();
        }

        // Seems like an old method... In the actual code, the method is the same as OfProject.
        IEnumerable<Models.Runner> IRunnerClient.GetAvailableRunners(int projectId)
        {
            return OfProject(projectId);
        }

        public IEnumerable<Models.Runner> GetAllRunnersWithScope(RunnerScope scope)
        {
            throw new NotImplementedException();
        }

        public Models.Runner EnableRunner(int projectId, RunnerId runnerId)
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var runner = GetServerRunner(runnerId.Id);

            var runnerReference = new RunnerRef(runner);

            if (project.EnabledRunners.Contains(runnerReference))
            {
                throw new GitLabException("Bad Request. Runner has already been taken");
            }

            project.EnabledRunners.Add(runnerReference);
            return runner.ToClientRunner();
        }

        public void DisableRunner(int projectId, RunnerId runnerId)
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

        private IEnumerable<Runner> GetOwnedRunners()
        {
            var projects = Server.AllProjects.Where(project => project.CanUserEditProject(Context.User));
            var runners = projects.SelectMany(p => p.RegisteredRunners);
            return runners;
        }

        private Runner GetServerRunner(int id)
        {
            return GetOwnedRunners().FirstOrDefault(runner => runner.Id == id) ?? throw new GitLabNotFoundException();
        }
    }
}
