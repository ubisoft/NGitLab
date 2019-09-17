using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ContributorClient : ClientBase, IContributorClient
    {
        private readonly int _projectId;

        public ContributorClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public IEnumerable<Contributor> All => throw new NotImplementedException();
    }
}
