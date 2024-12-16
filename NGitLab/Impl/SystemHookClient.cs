using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public sealed class SystemHookClient : ISystemHookClient
{
    private readonly API _api;
    private readonly string _path;

    public SystemHookClient(API api)
    {
        _api = api;
        _path = "hooks";
    }

    public IEnumerable<SystemHook> All => _api.Get().GetAll<SystemHook>(_path);

    public SystemHook this[long hookId] => _api.Get().To<SystemHook>(_path + "/" + hookId.ToStringInvariant());

    public SystemHook Create(SystemHookUpsert hook) => _api.Post().With(hook).To<SystemHook>(_path);

    public void Delete(long hookId)
    {
        _api.Delete().Execute(_path + "/" + hookId.ToStringInvariant());
    }
}
