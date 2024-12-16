using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class NamespacesClient : ClientBase, INamespacesClient
{
    public NamespacesClient(ClientContext context)
        : base(context)
    {
    }

    public Namespace this[long id] => throw new NotImplementedException();

    public Namespace this[string fullPath] => throw new NotImplementedException();

    public IEnumerable<Namespace> Accessible => throw new NotImplementedException();

    public IEnumerable<Namespace> Search(string search)
    {
        throw new NotImplementedException();
    }
}
