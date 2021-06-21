using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProtectedBranchTests
    {
        [Test]
        public async Task ProtectBranch_Test()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var branchClient = context.Client.GetRepository(project.Id).Branches;
            var branch = branchClient.Create(new BranchCreate() { Name = "protectedBranch", Ref = project.DefaultBranch });
            var protectedBranchClient = context.Client.GetProtectedBranchClient(project.Id);
            var branchProtect = new BranchProtect(branch.Name)
            {
                PushAccessLevel = AccessLevel.Maintainer,
                MergeAccessLevel = AccessLevel.NoAccess,
                AllowForcePush = true,
                AllowedToPush = new AccessLevelInfo[]
                {
                    new AccessLevelInfo()
                    {
                        AccessLevel = AccessLevel.Admin,
                        Description = "Admin",
                    },
                },
                AllowedToUnprotect = new AccessLevelInfo[]
                {
                    new AccessLevelInfo()
                    {
                        AccessLevel = AccessLevel.NoAccess,
                        Description = "Example",
                    },
                },
            };

            // Protect branch
            ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranchClient.ProtectBranch(branchProtect));

            // Get branch
            ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranchClient.GetProtectedBranch(branch.Name));

            // Get branches
            Assert.IsNotEmpty(protectedBranchClient.GetProtectedBranches());
            var protectedBranches = protectedBranchClient.GetProtectedBranches(branch.Name);
            Assert.IsNotEmpty(protectedBranches);
            ProtectedBranchAndBranchProtectAreEquals(branchProtect, protectedBranches[0]);

            // Unprotect branch
            protectedBranchClient.UnprotectBranch(branch.Name);
            Assert.IsEmpty(protectedBranchClient.GetProtectedBranches(branch.Name));
        }

        private void ProtectedBranchAndBranchProtectAreEquals(BranchProtect branchProtect, ProtectedBranch protectedBranch)
        {
            Assert.AreEqual(branchProtect.BranchName, protectedBranch.Name);
            Assert.AreEqual(branchProtect.PushAccessLevel, protectedBranch.PushAccessLevels[0].AccessLevel);
            Assert.AreEqual(branchProtect.MergeAccessLevel, protectedBranch.MergeAccessLevels[0].AccessLevel);
            Assert.AreEqual(branchProtect.AllowForcePush, protectedBranch.AllowForcePush);
        }
    }
}
