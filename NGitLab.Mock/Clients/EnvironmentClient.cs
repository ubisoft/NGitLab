using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class EnvironmentClient : ClientBase, IEnvironmentClient
    {
        private readonly int _projectId;

        public EnvironmentClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public IEnumerable<EnvironmentInfo> All => throw new NotImplementedException();

        public EnvironmentInfo Create(string name, string externalUrl)
        {
            throw new NotImplementedException();
        }

        public void Delete(int environmentId)
        {
            throw new NotImplementedException();
        }

        public EnvironmentInfo Edit(int environmentId, string name, string externalUrl)
        {
            throw new NotImplementedException();
        }

        public EnvironmentInfo Stop(int environmentId)
        {
            throw new NotImplementedException();
        }

        public GitLabCollectionResponse<EnvironmentInfo> GetEnvironmentsAsync(EnvironmentQuery query)
        {
            throw new NotImplementedException();
        }

        public EnvironmentInfo GetById(int environmentId)
        {
            throw new NotImplementedException();
        }

        public Task<EnvironmentInfo> GetByIdAsync(int environmentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
