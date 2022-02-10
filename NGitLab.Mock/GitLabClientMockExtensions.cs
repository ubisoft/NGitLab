using System;

namespace NGitLab.Mock
{
    public static class GitLabClientMockExtensions
    {
        public static IGitLabClient WithGraphQLClient(this IGitLabClient client, IGraphQLClient graphQLClient)
        {
            if (client is not Clients.GitLabClient gitLabClient)
                throw new ArgumentException("client must be a mock client", nameof(client));

            gitLabClient.GraphQL = graphQLClient;
            return client;
        }
    }
}
