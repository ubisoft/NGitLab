using System;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class AdvancedSearchClient : ISearchClient
{
    private readonly ClientContext _context;

    public AdvancedSearchClient(ClientContext context)
    {
        _context = context;
    }

    public GitLabCollectionResponse<SearchBlob> GetBlobsAsync(SearchQuery query)
    {
        throw new NotImplementedException();
    }
}
