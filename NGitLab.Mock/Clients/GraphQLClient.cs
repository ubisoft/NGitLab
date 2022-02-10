using System;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class GraphQLClient : ClientBase, IGraphQLClient
    {
        public GraphQLClient(ClientContext context)
            : base(context)
        {
        }

        public Task<T> ExecuteAsync<T>(GraphQLQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
