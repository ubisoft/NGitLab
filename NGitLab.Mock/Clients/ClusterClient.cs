using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ClusterClient : ClientBase, IClusterClient
    {
        private readonly int _projectId;

        public ClusterClient(ClientContext context, ProjectId projectId)
            : base(context)
        {
            _projectId = Server.AllProjects.FindProject(projectId.ValueAsUriParameter).Id;
        }

        public IEnumerable<ClusterInfo> All => throw new NotImplementedException();
    }
}
