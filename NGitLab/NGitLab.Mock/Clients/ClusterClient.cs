using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ClusterClient : ClientBase, IClusterClient
    {
        private readonly int _projectId;

        public ClusterClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public IEnumerable<ClusterInfo> All => throw new NotImplementedException();
    }
}
