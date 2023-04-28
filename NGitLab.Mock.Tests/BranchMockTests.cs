using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class BranchMockTests
    {
        [Test]
        public void Test_search_branches()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, defaultBranch: "main", configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Commit for branch_1", sourceBranch: "branch_1"))
                .BuildServer();

            var client = server.CreateClient("user1");
            var branchClient = client.GetRepository(1).Branches;

            var branches = branchClient.Search("main").ToList();
            Assert.IsTrue(branches.Any(branch => string.Equals(branch.Name, "main", System.StringComparison.Ordinal)));
            Assert.IsFalse(branches.Any(branch => string.Equals(branch.Name, "branch_1", System.StringComparison.Ordinal)));

            branches = branchClient.Search("^main$").ToList();
            Assert.IsTrue(branches.Any(branch => string.Equals(branch.Name, "main", System.StringComparison.Ordinal)));
            Assert.IsFalse(branches.Any(branch => string.Equals(branch.Name, "branch_1", System.StringComparison.Ordinal)));

            branches = branchClient.Search("^branch").ToList();
            Assert.IsFalse(branches.Any(branch => string.Equals(branch.Name, "main", System.StringComparison.Ordinal)));
            Assert.IsTrue(branches.Any(branch => string.Equals(branch.Name, "branch_1", System.StringComparison.Ordinal)));

            branches = branchClient.Search("1$").ToList();
            Assert.IsFalse(branches.Any(branch => string.Equals(branch.Name, "main", System.StringComparison.Ordinal)));
            Assert.IsTrue(branches.Any(branch => string.Equals(branch.Name, "branch_1", System.StringComparison.Ordinal)));
        }
    }
}
