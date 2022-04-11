using NGitLab.Models;

namespace NGitLab
{
    public interface ISearchClient
    {
        GitLabCollectionResponse<SearchBlob> GetBlobs(SearchQuery query);
    }
}
