using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectHooksClient : ClientBase, IProjectHooksClient
{
    public long ProjectId { get; }

    public ProjectHooksClient(ClientContext context, long projectId)
        : base(context)
    {
        ProjectId = projectId;
    }

    public IEnumerable<Models.ProjectHook> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var hooks = GetProject(ProjectId, ProjectPermission.Edit).Hooks;
                return ToClientProjectHooks(hooks).ToList();
            }
        }
    }

    public Models.ProjectHook this[long hookId]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var hook = All.FirstOrDefault(h => h.Id == hookId) ?? throw new GitLabNotFoundException();
                return hook;
            }
        }
    }

    public Models.ProjectHook Create(ProjectHookUpsert hook)
    {
        using (Context.BeginOperationScope())
        {
            var projectHook = UpsertToHook(hook);

            GetProject(ProjectId, ProjectPermission.Edit).Hooks.Add(projectHook);
            return projectHook.ToClientProjectHook();
        }
    }

    public Models.ProjectHook Update(long hookId, ProjectHookUpsert hook)
    {
        using (Context.BeginOperationScope())
        {
            var currentHook = GetProject(ProjectId, ProjectPermission.Edit).Hooks.FirstOrDefault(h => h.Id == hookId) ?? throw new GitLabNotFoundException();

            currentHook.Url = hook.Url;
            currentHook.PushEvents = hook.PushEvents;
            currentHook.MergeRequestsEvents = hook.MergeRequestsEvents;
            currentHook.IssuesEvents = hook.IssuesEvents;
            currentHook.TagPushEvents = hook.TagPushEvents;
            currentHook.NoteEvents = hook.NoteEvents;
            currentHook.JobEvents = hook.JobEvents;
            currentHook.PipelineEvents = hook.PipelineEvents;
            currentHook.EnableSslVerification = hook.EnableSslVerification;

            return currentHook.ToClientProjectHook();
        }
    }

    public void Delete(long hookId)
    {
        using (Context.BeginOperationScope())
        {
            var projectHooks = GetProject(ProjectId, ProjectPermission.Edit).Hooks;
            var hook = projectHooks.FirstOrDefault(h => h.Id == hookId) ?? throw new GitLabNotFoundException();

            projectHooks.Remove(hook);
        }
    }

    private static IEnumerable<Models.ProjectHook> ToClientProjectHooks(IEnumerable<ProjectHook> hooks)
    {
        return hooks.Select(hook => hook.ToClientProjectHook());
    }

    private static ProjectHook UpsertToHook(ProjectHookUpsert hook)
    {
        var hookFromUpsert = new ProjectHook
        {
            Url = hook.Url,
            PushEvents = hook.PushEvents,
            MergeRequestsEvents = hook.MergeRequestsEvents,
            IssuesEvents = hook.IssuesEvents,
            TagPushEvents = hook.TagPushEvents,
            NoteEvents = hook.NoteEvents,
            JobEvents = hook.JobEvents,
            PipelineEvents = hook.PipelineEvents,
            EnableSslVerification = hook.EnableSslVerification,
        };

        return hookFromUpsert;
    }
}
