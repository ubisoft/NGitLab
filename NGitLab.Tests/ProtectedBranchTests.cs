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
            AllowedToPush = new[]
            {
                new AccessLevelInfo
                {
                    AccessLevel = AccessLevel.Admin,
                    Description = "Admin",
                },
            },
            AllowedToUnprotect = new[]
            {
                new AccessLevelInfo
                {
                    AccessLevel = AccessLevel.Admin,
                    Description = "Example",
                },
            },
        };

        // Protect branch
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranchClient.ProtectBranch(branchProtect));

        // Get branch
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranchClient.GetProtectedBranch(branch.Name));

        // Get branches
        Assert.That(protectedBranchClient.GetProtectedBranches(), Is.Not.Empty);
        var protectedBranches = protectedBranchClient.GetProtectedBranches(branch.Name);
        Assert.That(protectedBranches, Is.Not.Empty);
        ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranches[0]);

        // Unprotect branch
        protectedBranchClient.UnprotectBranch(branch.Name);
        Assert.That(protectedBranchClient.GetProtectedBranches(branch.Name), Is.Empty);
    }

    private static void ProtectedBranchAndBranchProtectAreEquals(BranchProtect branchProtect, ProtectedBranch protectedBranch)
    {
        Assert.That(protectedBranch.Name, Is.EqualTo(branchProtect.BranchName));
        Assert.That(protectedBranch.PushAccessLevels[0].AccessLevel, Is.EqualTo(branchProtect.PushAccessLevel));
        Assert.That(protectedBranch.MergeAccessLevels[0].AccessLevel, Is.EqualTo(branchProtect.MergeAccessLevel));
        Assert.That(protectedBranch.AllowForcePush, Is.EqualTo(branchProtect.AllowForcePush));
    }
}
