using System;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal class ProtectedBranchClient : ClientBase, IProtectedBranchClient
    {
        private readonly int _projectId;

        public ProtectedBranchClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public ProtectedBranch GetProtectedBranch(string branchName)
        {
            throw new NotImplementedException();
        }

        public ProtectedBranch[] GetProtectedBranches(string search = null)
        {
            throw new NotImplementedException();
        }

        public ProtectedBranch ProtectBranch(BranchProtect branchProtect)
        {
            throw new NotImplementedException();
        }

        public void UnprotectBranch(string branchName)
        {
            throw new NotImplementedException();
        }
    }
}
