using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class GroupHooksClient : ClientBase, IGroupHooksClient
{
    public int _groupId { get; }

    public GroupHooksClient(ClientContext context, GroupId groupId)
        : base(context)
    {
        _groupId = Server.AllGroups.FindGroup(groupId.ValueAsUriParameter()).Id;
    }

    public IEnumerable<Models.GroupHook> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var hooks = GetGroup(_groupId, GroupPermission.Edit).Hooks;
                return ToClientGroupHooks(hooks).ToList();
            }
        }
    }

    public Models.GroupHook this[int hookId]
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

    public Models.GroupHook Create(GroupHookUpsert hook)
    {
        using (Context.BeginOperationScope())
        {
            var groupHook = UpsertToHook(hook);

            GetGroup(_groupId, GroupPermission.Edit).Hooks.Add(groupHook);
            return groupHook.ToClientGroupHook();
        }
    }

    public Models.GroupHook Update(int hookId, GroupHookUpsert hook)
    {
        using (Context.BeginOperationScope())
        {
            var currentHook = GetGroup(_groupId, GroupPermission.Edit).Hooks.FirstOrDefault(h => h.Id == hookId) ?? throw new GitLabNotFoundException();

            currentHook.Url = hook.Url;
            currentHook.PushEvents = hook.PushEvents;
            currentHook.MergeRequestsEvents = hook.MergeRequestsEvents;
            currentHook.IssuesEvents = hook.IssuesEvents;
            currentHook.TagPushEvents = hook.TagPushEvents;
            currentHook.NoteEvents = hook.NoteEvents;
            currentHook.JobEvents = hook.JobEvents;
            currentHook.PipelineEvents = hook.PipelineEvents;
            currentHook.EnableSslVerification = hook.EnableSslVerification;

            return currentHook.ToClientGroupHook();
        }
    }

    public void Delete(int hookId)
    {
        using (Context.BeginOperationScope())
        {
            var groupHooks = GetGroup(_groupId, GroupPermission.Edit).Hooks;
            var hook = groupHooks.FirstOrDefault(h => h.Id == hookId) ?? throw new GitLabNotFoundException();

            groupHooks.Remove(hook);
        }
    }

    private static IEnumerable<Models.GroupHook> ToClientGroupHooks(IEnumerable<GroupHook> hooks)
    {
        return hooks.Select(hook => hook.ToClientGroupHook());
    }

    private static GroupHook UpsertToHook(GroupHookUpsert hook)
    {
        var hookFromUpsert = new GroupHook
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
