using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class GroupHooksClient : ClientBase, IGroupHooksClient
{
    public long _groupId { get; }

    public GroupHooksClient(ClientContext context, GroupId groupId)
        : base(context)
    {
        _groupId = Server.AllGroups.FindGroup(groupId.ValueAsString()).Id;
    }

    public IEnumerable<Models.GroupHook> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return GetAllLockless();
            }
        }
    }

    private List<Models.GroupHook> GetAllLockless()
    {
        var hooks = GetGroup(_groupId, GroupPermission.Edit).Hooks;
        return ToClientGroupHooks(hooks).ToList();
    }

    public Models.GroupHook this[long hookId]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var hook = GetAllLockless().FirstOrDefault(h => h.Id == hookId) ?? throw GitLabException.NotFound();
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

    public Models.GroupHook Update(long hookId, GroupHookUpsert hook)
    {
        using (Context.BeginOperationScope())
        {
            var currentHook = GetGroup(_groupId, GroupPermission.Edit).Hooks.FirstOrDefault(h => h.Id == hookId) ?? throw GitLabException.NotFound();

            currentHook.Url = hook.Url;
            currentHook.PushEvents = hook.PushEvents ?? false;
            currentHook.MergeRequestsEvents = hook.MergeRequestsEvents ?? false;
            currentHook.IssuesEvents = hook.IssuesEvents ?? false;
            currentHook.TagPushEvents = hook.TagPushEvents ?? false;
            currentHook.NoteEvents = hook.NoteEvents ?? false;
            currentHook.JobEvents = hook.JobEvents ?? false;
            currentHook.PipelineEvents = hook.PipelineEvents ?? false;
            currentHook.WikiPagesEvents = hook.WikiPagesEvents ?? false;
            currentHook.EnableSslVerification = hook.EnableSslVerification ?? false;
            currentHook.Token = currentHook.Token;

            return currentHook.ToClientGroupHook();
        }
    }

    public void Delete(long hookId)
    {
        using (Context.BeginOperationScope())
        {
            var groupHooks = GetGroup(_groupId, GroupPermission.Edit).Hooks;
            var hook = groupHooks.FirstOrDefault(h => h.Id == hookId) ?? throw GitLabException.NotFound();

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
            PushEvents = hook.PushEvents ?? false,
            MergeRequestsEvents = hook.MergeRequestsEvents ?? false,
            IssuesEvents = hook.IssuesEvents ?? false,
            TagPushEvents = hook.TagPushEvents ?? false,
            NoteEvents = hook.NoteEvents ?? false,
            JobEvents = hook.JobEvents ?? false,
            PipelineEvents = hook.PipelineEvents ?? false,
            WikiPagesEvents = hook.WikiPagesEvents ?? false,
            EnableSslVerification = hook.EnableSslVerification ?? false,
            Token = hook.Token,
        };

        return hookFromUpsert;
    }
}
