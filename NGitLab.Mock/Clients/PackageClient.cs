using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class PackageClient : ClientBase, IPackageClient
    {
        public PackageClient(ClientContext context)
            : base(context)
        {
        }

        public Task<Package> PublishAsync(int projectId, PackagePublish packagePublish, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PackageSearchResult> Get(int projectId, PackageQuery packageQuery)
        {
            throw new System.NotImplementedException();
        }

        public Task<PackageSearchResult> GetByIdAsync(int projectId, int packageId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
