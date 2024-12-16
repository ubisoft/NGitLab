using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class GroupHooksClient : IGroupHooksClient
{
    private readonly API _api;
    private readonly string _path;

    public GroupHooksClient(API api, GroupId groupId)
    {
        _api = api;
        _path = $"{Group.Url}/{groupId.ValueAsUriParameter()}/hooks";
    }

    public IEnumerable<GroupHook> All => _api.Get().GetAll<GroupHook>(_path);

    public GroupHook this[long hookId] => _api.Get().To<GroupHook>(_path + "/" + hookId.ToStringInvariant());

    public GroupHook Create(GroupHookUpsert hook) => _api.Post().With(hook).To<GroupHook>(_path);

    public GroupHook Update(long hookId, GroupHookUpsert hook) => _api.Put().With(hook).To<GroupHook>(_path + "/" + hookId.ToStringInvariant());

    public void Delete(long hookId)
    {
        _api.Delete().Execute(_path + "/" + hookId.ToStringInvariant());
    }
}
