using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class WikiClient : ClientBase, IWikiClient
{
    private readonly long _projectId;

    public WikiClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public WikiPage this[string slug] => throw new NotImplementedException();

    public IEnumerable<WikiPage> All => throw new NotImplementedException();

    public WikiPage Create(WikiPageCreate wikiPage)
    {
        throw new NotImplementedException();
    }

    public void Delete(string slug)
    {
        throw new NotImplementedException();
    }

    public WikiPage Update(string slug, WikiPageUpdate wikiPage)
    {
        throw new NotImplementedException();
    }
}
