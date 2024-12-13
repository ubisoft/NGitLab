using System;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class GroupSearchClient : ClientBase, ISearchClient
{
    private readonly ClientContext _context;
    private readonly long _groupId;

    public GroupSearchClient(ClientContext context, GroupId groupId)
        : base(context)
    {
        _context = context;
        _groupId = Server.AllGroups.FindGroup(groupId.ValueAsString()).Id;
    }

    public GitLabCollectionResponse<SearchBlob> GetBlobsAsync(SearchQuery query)
    {
        throw new NotImplementedException();
    }
}
