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

    public Models.ProtectedBranch UpdateProtectedBranch(string branchName, ProtectedBranchUpdate protectedBranchUpdate)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Edit);
            var protectedBranch = project.ProtectedBranches.First(b => b.Name.Equals(branchName, StringComparison.Ordinal));

            if (protectedBranchUpdate.CodeOwnerApprovalRequired is not null)
            {
                protectedBranch.CodeOwnerApprovalRequired = protectedBranchUpdate.CodeOwnerApprovalRequired.Value;
            }

            if (protectedBranchUpdate.AllowForcePush is not null)
            {
                protectedBranch.AllowForcePush = protectedBranchUpdate.AllowForcePush.Value;
            }

            if (protectedBranchUpdate.AllowedToMerge is not null)
            {
                foreach (var allowedToMerge in protectedBranchUpdate.AllowedToMerge)
                {
                    if (allowedToMerge.Id is not null && allowedToMerge.Destroy is true)
                    {
                        protectedBranch.MergeAccessLevels = protectedBranch.MergeAccessLevels.Where(l => l.Id != allowedToMerge.Id).ToArray();
                    }
                    else if (allowedToMerge.Id is not null && allowedToMerge.Destroy is false && allowedToMerge.AccessLevel is not null)
                    {
                        protectedBranch.MergeAccessLevels.First(l => l.Id == allowedToMerge.Id).AccessLevel =
                            allowedToMerge.AccessLevel.Value;
                    }
                    else if (allowedToMerge.Id is null && allowedToMerge.Destroy is false)
                    {
                        var accessLevel = new AccessLevelInfo();

                        if (allowedToMerge.AccessLevel is not null)
                        {
                            accessLevel.AccessLevel = allowedToMerge.AccessLevel.Value;
                        }

                        accessLevel.Description = allowedToMerge.Description;

                        if (allowedToMerge.GroupId is not null)
                        {
                            accessLevel.GroupId = allowedToMerge.GroupId.Value;
                        }

                        if (allowedToMerge.UserId is not null)
                        {
                            accessLevel.UserId = allowedToMerge.UserId.Value;
                        }

                        protectedBranch.MergeAccessLevels =
                            protectedBranch.MergeAccessLevels.Concat([accessLevel]).ToArray();
                    }

                }
            }

            if (protectedBranchUpdate.AllowedToPush is not null)
            {
                foreach (var allowedToPush in protectedBranchUpdate.AllowedToPush)
                {
                    if (allowedToPush.Id is not null && allowedToPush.Destroy is true)
                    {
                        protectedBranch.PushAccessLevels = protectedBranch.PushAccessLevels.Where(l => l.Id != allowedToPush.Id).ToArray();
                    }
                    else if (allowedToPush.Id is not null && allowedToPush.Destroy is false && allowedToPush.AccessLevel is not null)
                    {
                        protectedBranch.PushAccessLevels.First(l => l.Id == allowedToPush.Id).AccessLevel =
                            allowedToPush.AccessLevel.Value;
                    }
                    else if (allowedToPush.Id is null && allowedToPush.Destroy is false)
                    {
                        var accessLevel = new AccessLevelInfo();

                        if (allowedToPush.AccessLevel is not null)
                        {
                            accessLevel.AccessLevel = allowedToPush.AccessLevel.Value;
                        }

                        accessLevel.Description = allowedToPush.Description;

                        if (allowedToPush.GroupId is not null)
                        {
                            accessLevel.GroupId = allowedToPush.GroupId.Value;
                        }

                        if (allowedToPush.UserId is not null)
                        {
                            accessLevel.UserId = allowedToPush.UserId.Value;
                        }

                        protectedBranch.PushAccessLevels =
                            protectedBranch.PushAccessLevels.Concat([accessLevel]).ToArray();
                    }

                }
            }

            return protectedBranch.ToProtectedBranchClient();
        }

    }
}
