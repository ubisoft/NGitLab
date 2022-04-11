using System;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class GroupSearchClient : ISearchClient
    {
        private readonly ClientContext _context;
        private readonly int _groupId;

        public GroupSearchClient(ClientContext context, int groupId)
        {
            _context = context;
            _groupId = groupId;
        }

        public GitLabCollectionResponse<SearchBlob> GetBlobs(SearchQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
