using System;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProtectedBranchClient : ClientBase, IProtectedBranchClient
{
    private readonly long _projectId;

    public ProtectedBranchClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Models.ProtectedBranch GetProtectedBranch(string branchName)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Edit);
            return project.ProtectedBranches.First(b => b.Name.Equals(branchName, StringComparison.Ordinal)).ToProtectedBranchClient();
        }
    }

    public Models.ProtectedBranch[] GetProtectedBranches(string search = null)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Edit);
            return project.ProtectedBranches
                       .Where(b => b.Name.Contains(search ?? "", StringComparison.Ordinal))
                       .Select(b => b.ToProtectedBranchClient())
                       .ToArray();
        }
    }

    public Models.ProtectedBranch ProtectBranch(BranchProtect branchProtect)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Edit);
            var protectedBranch = project.ProtectedBranches.FirstOrDefault(b => b.Name.Equals(branchProtect.BranchName, StringComparison.Ordinal));
            if (protectedBranch == null)
            {
                protectedBranch = new();
                project.ProtectedBranches.Add(protectedBranch);
            }

            protectedBranch.Name = branchProtect.BranchName;
            protectedBranch.AllowForcePush = branchProtect.AllowForcePush;
            protectedBranch.CodeOwnerApprovalRequired = branchProtect.CodeOwnerApprovalRequired;

            return protectedBranch.ToProtectedBranchClient();
        }
    }

    public void UnprotectBranch(string branchName)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Edit);
            var protectedBranch = project.ProtectedBranches.FirstOrDefault(b => b.Name.Equals(branchName, StringComparison.Ordinal));
            if (protectedBranch != null)
            {
                project.ProtectedBranches.Remove(protectedBranch);
            }
        }
    }
}
