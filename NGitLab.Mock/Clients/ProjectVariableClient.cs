using System;
using System.Collections.Generic;
using System.ComponentModel;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectVariableClient : ClientBase, IProjectVariableClient
{
    private readonly int _projectId;

    public ProjectVariableClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Variable this[string key, string environmentScope = null] => throw new NotImplementedException();

    public IEnumerable<Variable> All => throw new NotImplementedException();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Variable Create(VariableCreate model)
    {
        throw new NotImplementedException();
    }

    public Variable Create(Variable model)
    {
        throw new NotImplementedException();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Delete(string key) => Delete(key, null);

    public void Delete(string key, string environmentScope)
    {
        throw new NotImplementedException();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Variable Update(string key, VariableUpdate model) => Update(key, model, null);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Variable Update(string key, VariableUpdate model, string environmentScope)
    {
        throw new NotImplementedException();
    }

    public Variable Update(string key, Variable model) => Update(key, model, null);

    public Variable Update(string key, Variable model, string environmentScope)
    {
        throw new NotImplementedException();
    }
}
