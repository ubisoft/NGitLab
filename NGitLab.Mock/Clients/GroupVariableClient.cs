using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class GroupVariableClient : ClientBase, IGroupVariableClient
{
    private readonly long _groupId;

    public GroupVariableClient(ClientContext context, GroupId groupId)
        : base(context)
    {
        _groupId = Server.AllGroups.FindGroup(groupId.ValueAsString()).Id;
    }

    public Variable this[string key] => this[key, null];

    public Variable this[string key, string environmentScope] => throw new NotImplementedException();

    public IEnumerable<Variable> All => throw new NotImplementedException();

    public Variable Create(VariableCreate model)
    {
        throw new NotImplementedException();
    }

    public void Delete(string key) => Delete(key, null);

    public void Delete(string key, string environmentScope)
    {
        throw new NotImplementedException();
    }

    public Variable Update(string key, VariableUpdate model) => Update(key, null, model);

    public Variable Update(string key, string environmentScope, VariableUpdate model)
    {
        throw new NotImplementedException();
    }
}
