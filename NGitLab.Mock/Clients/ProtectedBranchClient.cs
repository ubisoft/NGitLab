using System;
using System.Collections.Generic;
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
                UpdateAccessLevels(
                    protectedBranchUpdate.AllowedToMerge,
                    (id, level) => protectedBranch.MergeAccessLevels.First(l => l.Id == id).AccessLevel = level,
                    newAccessLevel => protectedBranch.MergeAccessLevels = protectedBranch.MergeAccessLevels.Concat([newAccessLevel]).ToArray(),
                    id => protectedBranch.MergeAccessLevels = protectedBranch.MergeAccessLevels.Where(l => l.Id != id).ToArray());
            }

            if (protectedBranchUpdate.AllowedToPush is not null)
            {
                UpdateAccessLevels(
                    protectedBranchUpdate.AllowedToPush,
                    (id, level) => protectedBranch.PushAccessLevels.First(l => l.Id == id).AccessLevel = level,
                    newAccessLevel => protectedBranch.PushAccessLevels = protectedBranch.PushAccessLevels.Concat([newAccessLevel]).ToArray(),
                    id => protectedBranch.PushAccessLevels = protectedBranch.PushAccessLevels.Where(l => l.Id != id).ToArray());
            }

            return protectedBranch.ToProtectedBranchClient();
        }
    }

    private void UpdateAccessLevels(IEnumerable<AccessLevelUpdate> accessLevels, Action<int, AccessLevel> updateAccessLevel, Action<AccessLevelInfo> addAccessLevel, Action<int> removeAccessLevel)
    {
        foreach (var accessLevel in accessLevels)
        {
            UpdateAccessLevel(updateAccessLevel, addAccessLevel, removeAccessLevel, accessLevel);
        }
    }

    private static void UpdateAccessLevel(Action<int, AccessLevel> updateAccessLevel, Action<AccessLevelInfo> addAccessLevel, Action<int> removeAccessLevel,
        AccessLevelUpdate accessLevel)
    {
        if (accessLevel.Id is not null && accessLevel.Destroy is true)
        {
            removeAccessLevel(accessLevel.Id.Value);
            return;
        }

        if (accessLevel.Id is not null && accessLevel.Destroy is false && accessLevel.AccessLevel is not null)
        {
            updateAccessLevel(accessLevel.Id.Value, accessLevel.AccessLevel.Value);
            return;
        }

        if (accessLevel.Id is not null || accessLevel.Destroy is not false) return;

        var newAccessLevel = new AccessLevelInfo();

        if (accessLevel.AccessLevel is not null)
        {
            accessLevel.AccessLevel = accessLevel.AccessLevel.Value;
        }

        accessLevel.Description = accessLevel.Description;

        if (accessLevel.GroupId is not null)
        {
            accessLevel.GroupId = accessLevel.GroupId.Value;
        }

        if (accessLevel.UserId is not null)
        {
            accessLevel.UserId = accessLevel.UserId.Value;
        }

        addAccessLevel(newAccessLevel);
    }
}
