using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class TriggerClient : ITriggerClient
{
    private readonly API _api;
    private readonly string _triggersPath;

    public TriggerClient(API api, ProjectId projectId)
    {
        _api = api;
        _triggersPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}/triggers";
    }

    public Trigger this[long id] => _api.Get().To<Trigger>(_triggersPath + "/" + id.ToStringInvariant());

    public IEnumerable<Trigger> All => _api.Get().GetAll<Trigger>(_triggersPath);

    public Trigger Create(string description)
    {
        return _api.Post().To<Trigger>($"{_triggersPath}?description={description}");
    }
}
