using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ReleaseLinkClient : ClientBase, IReleaseLinkClient
{
    private readonly long _projectId;
    private readonly string _tagName;

    public ReleaseLinkClient(ClientContext context, long projectId, string tagName)
        : base(context)
    {
        _projectId = projectId;
        _tagName = tagName;
    }

    public ReleaseLink this[long id] => throw new NotImplementedException();

    public IEnumerable<ReleaseLink> All => throw new NotImplementedException();

    public ReleaseLink Create(ReleaseLinkCreate data)
    {
        throw new NotImplementedException();
    }

    public void Delete(long id)
    {
        throw new NotImplementedException();
    }

    public ReleaseLink Update(long id, ReleaseLinkUpdate data)
    {
        throw new NotImplementedException();
    }
}
