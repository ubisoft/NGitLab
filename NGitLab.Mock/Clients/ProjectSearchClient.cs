using System;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ProjectSearchClient : ISearchClient
    {
        private readonly ClientContext _context;
        private readonly int _projectId;

        public ProjectSearchClient(ClientContext context, int projectId)
        {
            _context = context;
            _projectId = projectId;
        }

        public GitLabCollectionResponse<SearchBlob> GetBlobsAsync(SearchQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
