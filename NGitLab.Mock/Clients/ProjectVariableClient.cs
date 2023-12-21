using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectVariableClient : ClientBase, IProjectVariableClient
{
    private readonly int _projectId;

    public ProjectVariableClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsUriParameter()).Id;
    }

    public Variable this[string key] => throw new NotImplementedException();

    public IEnumerable<Variable> All => throw new NotImplementedException();

    public Variable Create(VariableCreate model)
    {
        throw new NotImplementedException();
    }

    public void Delete(string key)
    {
        throw new NotImplementedException();
    }

    public Variable Update(string key, VariableUpdate model)
    {
        throw new NotImplementedException();
    }
}
