using NGitLab.Models;

namespace NGitLab;

public interface ISearchClient
{
    GitLabCollectionResponse<SearchBlob> GetBlobsAsync(SearchQuery query);
}
