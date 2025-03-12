using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class ProtectedBranchTests
{
    [Test]
    [NGitLabRetry]
    public async Task ProtectBranch_Test()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branchClient = context.Client.GetRepository(project.Id).Branches;
        var branch = branchClient.Create(new BranchCreate { Name = $"protectedBranch-{Guid.NewGuid()}", Ref = project.DefaultBranch });
        var protectedBranchClient = context.Client.GetProtectedBranchClient(project.Id);
        var branchProtect = new BranchProtect(branch.Name)
        {
            PushAccessLevel = AccessLevel.Maintainer,
            MergeAccessLevel = AccessLevel.NoAccess,
            AllowForcePush = true,
            AllowedToPush = new[] { new AccessLevelInfo { AccessLevel = AccessLevel.Admin, Description = "Admin", }, },
            AllowedToMerge = new[] { new AccessLevelInfo { AccessLevel = AccessLevel.Admin, Description = "Admin", }, },
            AllowedToUnprotect = new[]
            {
                new AccessLevelInfo { AccessLevel = AccessLevel.Admin, Description = "Example", },
            },
        };

        // Protect branch
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranchClient.ProtectBranch(branchProtect));

        // Get branch
        var existingBranch = protectedBranchClient.GetProtectedBranch(branch.Name);
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, existingBranch);

        // Get branches
        Assert.That(protectedBranchClient.GetProtectedBranches(), Is.Not.Empty);
        var protectedBranches = protectedBranchClient.GetProtectedBranches(branch.Name);
        Assert.That(protectedBranches, Is.Not.Empty);
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranches[0]);

        // Update protected branch - test delete and update
        var protectedBranchUpdate = new ProtectedBranchUpdate
        {
            AllowForcePush = false,
            AllowedToPush = new[]
            {
                new AccessLevelUpdate { Id = existingBranch.PushAccessLevels.First(l => l.AccessLevel == AccessLevel.Admin).Id, Destroy = true, }, // This existing one should be deleted
            },
            AllowedToMerge = new[]
            {
                new AccessLevelUpdate { Id = existingBranch.MergeAccessLevels.First(l => l.AccessLevel == AccessLevel.Admin).Id, AccessLevel = AccessLevel.Maintainer } // The existing one should be updated
            }
        };
        var updatedBranch = protectedBranchClient.UpdateProtectedBranch(branch.Name, protectedBranchUpdate);
        Assert.That(updatedBranch.AllowForcePush, Is.EqualTo(protectedBranchUpdate.AllowForcePush));
        Assert.That(updatedBranch.PushAccessLevels.Count(l => l.AccessLevel == AccessLevel.Admin), Is.EqualTo(0));
        Assert.That(updatedBranch.MergeAccessLevels.Count(l => l.AccessLevel == AccessLevel.Admin), Is.EqualTo(0));
        Assert.That(updatedBranch.MergeAccessLevels.Count(l => l.AccessLevel == AccessLevel.Maintainer), Is.EqualTo(1));

        // Update protected branch - test create
        protectedBranchUpdate = new ProtectedBranchUpdate
        {
            AllowedToPush = new[]
            {
                new AccessLevelUpdate { AccessLevel = AccessLevel.Admin }, // This one should be created
            }
        };
        updatedBranch = protectedBranchClient.UpdateProtectedBranch(branch.Name, protectedBranchUpdate);
        Assert.That(updatedBranch.PushAccessLevels.Count(l => l.AccessLevel == AccessLevel.Admin), Is.EqualTo(1));

        // Unprotect branch
        protectedBranchClient.UnprotectBranch(branch.Name);
        Assert.That(protectedBranchClient.GetProtectedBranches(branch.Name), Is.Empty);
    }

    private static void ProtectedBranchAndBranchProtectAreEquals(BranchProtect branchProtect,
        ProtectedBranch protectedBranch)
    {
        Assert.That(protectedBranch.Name, Is.EqualTo(branchProtect.BranchName));
        Assert.That(protectedBranch.PushAccessLevels[0].AccessLevel, Is.EqualTo(branchProtect.PushAccessLevel));
        Assert.That(protectedBranch.MergeAccessLevels[0].AccessLevel, Is.EqualTo(branchProtect.MergeAccessLevel));
        Assert.That(protectedBranch.AllowForcePush, Is.EqualTo(branchProtect.AllowForcePush));
        Assert.That(protectedBranch.CodeOwnerApprovalRequired, Is.EqualTo(branchProtect.CodeOwnerApprovalRequired));
    }
}
