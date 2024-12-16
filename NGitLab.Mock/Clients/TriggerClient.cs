using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class TriggerClient : ClientBase, ITriggerClient
{
    private readonly long _projectId;

    public TriggerClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Trigger this[long id] => throw new NotImplementedException();

    public IEnumerable<Trigger> All => throw new NotImplementedException();

    public Trigger Create(string description)
    {
        throw new NotImplementedException();
    }
}
