using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class SearchClient : ISearchClient
{
    private readonly API _api;
    private readonly string _baseUrl;

    public SearchClient(API api, string baseUrl)
    {
        _api = api;
        _baseUrl = baseUrl;
    }

    public GitLabCollectionResponse<SearchBlob> GetBlobsAsync(SearchQuery query)
    {
        var url = CreateGetUrl("blobs", query);
        return _api.Get().GetAllAsync<SearchBlob>(url);
    }

    private string CreateGetUrl(string scope, SearchQuery query)
    {
        var url = _baseUrl;

        url = Utils.AddParameter(url, "scope", scope);
        url = Utils.AddParameter(url, "search", query.Search);

        if (query.PerPage.HasValue)
        {
            url = Utils.AddParameter(url, "per_page", query.PerPage);
        }

        if (query.Page.HasValue)
        {
            url = Utils.AddParameter(url, "page", query.Page);
        }

        if (query.Confidential.HasValue)
        {
            url = Utils.AddParameter(url, "confidential", query.Confidential);
        }

        if (!string.IsNullOrEmpty(query.State))
        {
            url = Utils.AddParameter(url, "state", query.State);
        }

        if (!string.IsNullOrEmpty(query.OrderBy))
        {
            url = Utils.AddOrderBy(url, query.OrderBy);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            url = Utils.AddParameter(url, "sort", query.Sort);
        }

        return url;
    }
}
