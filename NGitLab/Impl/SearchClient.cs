using System;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class SearchClient : ISearchClient
    {
        private readonly API _api;
        private readonly string _baseUrl;

        public SearchClient(API api, string baseUrl)
        {
            _api = api;
            _baseUrl = baseUrl;
        }

        public GitLabCollectionResponse<SearchBlob> GetBlobs(SearchQuery query)
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

            if (query.Confidential.HasValue)
            {
                if (!string.Equals(scope, "issues", StringComparison.Ordinal))
                {
                    throw new NotSupportedException("Confidential is only supported for 'issues' scope");
                }

                url = Utils.AddParameter(url, "confidential", query.Confidential);
            }

            if (!string.IsNullOrEmpty(query.State))
            {
                if (string.Equals(scope, "issues", StringComparison.Ordinal) || string.Equals(scope, "merge_requests", StringComparison.Ordinal))
                {
                    throw new NotSupportedException("State is only supported for 'issues' or 'merge_requests' scope");
                }

                url = Utils.AddParameter(url, "state", query.State);
            }

            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                if (!string.Equals(query.OrderBy, "created_at", StringComparison.Ordinal))
                {
                    throw new NotSupportedException("Only allowed value for OrderBy is 'created_at'");
                }

                url = Utils.AddOrderBy(url, query.OrderBy);
            }

            if (!string.IsNullOrEmpty(query.Sort))
            {
                if (!string.Equals(query.Sort, "asc", StringComparison.Ordinal) &&
                    !string.Equals(query.Sort, "desc", StringComparison.Ordinal))
                {
                    throw new NotSupportedException("Only allowed values for Sort are 'asc' and 'desc'");
                }
            }

            return url;
        }
    }
}
