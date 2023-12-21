using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class BranchesMockTests
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
        var expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo("main"));

        branches = branchClient.Search("^main$").ToList();
        expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo("main"));

        branches = branchClient.Search("^branch").ToList();
        expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo("branch_1"));

        branches = branchClient.Search("1$").ToList();
        expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo("branch_1"));

        branches = branchClient.Search("foobar").ToList();
        Assert.That(branches, Is.Empty);
    }
}
