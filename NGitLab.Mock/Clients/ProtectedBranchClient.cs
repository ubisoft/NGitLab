using System;
using System.Linq;
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

        public Models.ProtectedBranch GetProtectedBranch(string branchName)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.ProtectedBranches.First(b => b.Name.Equals(branchName, StringComparison.Ordinal)).ToProtectedBranchClient();
            }
        }

        public Models.ProtectedBranch[] GetProtectedBranches(string search)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.ProtectedBranches.Select(b => b.ToProtectedBranchClient()).ToArray();
            }
        }

        public Models.ProtectedBranch ProtectBranch(BranchProtect branchProtect)
        {
            throw new NotImplementedException();
        }

        public void UnprotectBranch(string branchName)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var protectedBranches = project.ProtectedBranches.Select(b => b.ToProtectedBranchClient()).ToList();
                var branchToUnprotect = protectedBranches.SingleOrDefault(b => b.Name.Equals(branchName, StringComparison.Ordinal));
                if(branchToUnprotect != null)
                {
                    protectedBranches.Remove(branchToUnprotect);
                }
            }
        }
    }
}
