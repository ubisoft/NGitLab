using System.Collections.Generic;
using System.ComponentModel;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class TriggerClient : ITriggerClient
{
    private readonly API _api;
    private readonly string _triggersPath;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TriggerClient(API api, int projectId)
        : this(api, (long)projectId)
    {
    }

    public TriggerClient(API api, ProjectId projectId)
    {
        _api = api;
        _triggersPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}/triggers";
    }

    public Trigger this[int id] => _api.Get().To<Trigger>(_triggersPath + "/" + id.ToStringInvariant());

    public IEnumerable<Trigger> All => _api.Get().GetAll<Trigger>(_triggersPath);

    public Trigger Create(string description)
    {
        return _api.Post().To<Trigger>($"{_triggersPath}?description={description}");
    }
}
