using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IGraphQLClient
    {
        Task<T> ExecuteAsync<T>(GraphQLQuery query, CancellationToken cancellationToken = default);
    }
}
