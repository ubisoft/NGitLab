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
        var branch = branchClient.Create(new BranchCreate { Name = "protectedBranch", Ref = project.DefaultBranch });
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

        // Update protected branch
        var protectedBranchUpdate = new ProtectedBranchUpdate
        {
            AllowForcePush = false,
            CodeOwnerApprovalRequired = true,
            AllowedToPush = new[]
            {
                new AccessLevelUpdate { Id = existingBranch.PushAccessLevels[0].Id, Destroy = true, }, // This existing one should be removed
                new AccessLevelUpdate { AccessLevel = AccessLevel.Maintainer, Description = "Maintainer", }, // This one should be added
            },
            AllowedToMerge = new[]
            {
                new AccessLevelUpdate { Id = existingBranch.MergeAccessLevels[0].Id, AccessLevel = AccessLevel.Maintainer } // The existing one should be updated
            }
        };
        var updatedBranch = protectedBranchClient.UpdateProtectedBranch(branch.Name, protectedBranchUpdate);
        Assert.That(updatedBranch, Is.Not.Null);
        Assert.That(updatedBranch.Name, Is.EqualTo(branchProtect.BranchName));
        Assert.That(updatedBranch.AllowForcePush, Is.EqualTo(protectedBranchUpdate.AllowForcePush));
        Assert.That(updatedBranch.CodeOwnerApprovalRequired, Is.EqualTo(protectedBranchUpdate.CodeOwnerApprovalRequired));
        Assert.That(updatedBranch.PushAccessLevels[0].AccessLevel, Is.EqualTo(protectedBranchUpdate.AllowedToPush[1].AccessLevel));
        Assert.That(updatedBranch.MergeAccessLevels[0].AccessLevel, Is.EqualTo(protectedBranchUpdate.AllowedToMerge[0].AccessLevel));

        // Get branches
        Assert.That(protectedBranchClient.GetProtectedBranches(), Is.Not.Empty);
        var protectedBranches = protectedBranchClient.GetProtectedBranches(branch.Name);
        Assert.That(protectedBranches, Is.Not.Empty);
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranches[0]);

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
