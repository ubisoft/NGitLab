using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class SystemHookClient : ClientBase, ISystemHookClient
{
    public SystemHookClient(ClientContext context)
        : base(context)
    {
    }

    public Models.SystemHook this[long hookId]
    {
        get
        {
            AssertIsAdmin();
            using (Context.BeginOperationScope())
            {
                var result = Server.SystemHooks.FirstOrDefault(hook => hook.Id == hookId);
                if (result == null)
                    throw new GitLabNotFoundException();

                return result.ToClientSystemHook();
            }
        }
    }

    public IEnumerable<Models.SystemHook> All
    {
        get
        {
            AssertIsAdmin();
            using (Context.BeginOperationScope())
            {
                return Server.SystemHooks.Select(hook => hook.ToClientSystemHook()).ToList();
            }
        }
    }

    public Models.SystemHook Create(SystemHookUpsert hook)
    {
        AssertIsAdmin();
        using (Context.BeginOperationScope())
        {
            var newHook = new SystemHook
            {
                CreatedAt = DateTime.UtcNow,
                EnableSslVerification = hook.EnableSslVerification,
                MergeRequestsEvents = hook.MergeRequestsEvents,
                PushEvents = hook.PushEvents,
                TagPushEvents = hook.TagPushEvents,
                Url = hook.Url,
                RepositoryUpdateEvents = hook.RepositoryUpdateEvents,
            };
            Server.SystemHooks.Add(newHook);

            return newHook.ToClientSystemHook();
        }
    }

    public void Delete(long hookId)
    {
        AssertIsAdmin();
        using (Context.BeginOperationScope())
        {
            var result = Server.SystemHooks.FirstOrDefault(hook => hook.Id == hookId);
            if (result == null)
                throw new GitLabNotFoundException();

            Server.SystemHooks.Remove(result);
        }
    }

    private void AssertIsAdmin()
    {
        if (Context.IsAuthenticated && Context.User.IsAdmin)
            return;

        throw new GitLabException("User must be admin");
    }
}
