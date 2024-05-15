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
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Variable this[string key] => throw new NotImplementedException();

    public IEnumerable<Variable> All => throw new NotImplementedException();

    [Obsolete($"Use '{nameof(Create)}({nameof(Variable)} model)' instead")]
    public Variable Create(VariableCreate model)
    {
        throw new NotImplementedException();
    }

    public Variable Create(Variable model)
    {
        throw new NotImplementedException();
    }

    public void Delete(string key, string environmentScope = null)
    {
        throw new NotImplementedException();
    }

#if (NET || NET48_OR_GREATER || NETSTANDARD2_1_OR_GREATER)

    [Obsolete($"Use {nameof(Update)} with parameter {nameof(Variable)} instead")]
    public Variable Update(string key, VariableUpdate model, string environmentScope = null)
    {
        throw new NotImplementedException();
    }

    public Variable Update(string key, Variable model, string environmentScope = null)
    {
        throw new NotImplementedException();
    }
#else
    [Obsolete($"Use {nameof(Update)} with parameter {nameof(Variable)} instead")]
    public Variable Update(string key, VariableUpdate model) => Update(key, model, null);

    [Obsolete($"Use {nameof(Update)} with parameter {nameof(Variable)} instead")]
    public Variable Update(string key, VariableUpdate model, string environmentScope)
    {
        throw new NotImplementedException();
    }

    public Variable Update(string key, Variable model) => Update(key, model, null);

    public Variable Update(string key, Variable model, string environmentScope)
    {
        throw new NotImplementedException();
    }
#endif
}
