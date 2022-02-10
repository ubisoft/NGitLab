using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal sealed class GraphQLClient : IGraphQLClient
    {
        private readonly API _api;

        public GraphQLClient(API api)
        {
            _api = api;
        }

        public async Task<T> ExecuteAsync<T>(GraphQLQuery query, CancellationToken cancellationToken = default)
        {
            var result = await _api.Post().With(query).ToAsync<GraphQLResponse<T>>("/api/graphql", cancellationToken).ConfigureAwait(false);
            if (result.Errors is { Length: > 0 })
                throw new GitLabException(string.Join("\n", result.Errors.Select(error => error.Message)));

            return result.Data;
        }

        private sealed class GraphQLResponse<T>
        {
            [JsonPropertyName("data")]
            public T Data { get; set; }

            [JsonPropertyName("errors")]
            public GraphQLError[] Errors { get; set; }
        }

        private sealed class GraphQLError
        {
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}
