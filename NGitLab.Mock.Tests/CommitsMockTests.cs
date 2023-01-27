using System;
using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class CommitsMockTests
    {
        [Test]
        public void Test_commits_added_can_be_found()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("test-project", id: 1, configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Create branch", sourceBranch: "branch-01"))
                .BuildServer();

            var client = server.CreateClient();
            var commit = client.GetCommits(1).GetCommit("branch-01");

            Assert.AreEqual("Create branch", commit.Message.TrimEnd('\r', '\n'));
        }

        [Test]
        public void Test_commits_with_tags_can_be_found()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("test-project", id: 1, configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Changes with tag", tags: new[] { "1.0.0" }))
                .BuildServer();

            var client = server.CreateClient();
            var commit = client.GetCommits(1).GetCommit("1.0.0");

            Assert.AreEqual("Changes with tag", commit.Message.TrimEnd('\r', '\n'));
        }

        [Test]
        public void Test_tags_from_commit_can_be_found()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Changes with tag", tags: new[] { "1.0.0" }))
                .BuildServer();

            var client = server.CreateClient();
            var tags = client.GetRepository(1).Tags.All.ToArray();

            Assert.That(tags, Has.One.Items);
            Assert.AreEqual("1.0.0", tags[0].Name);
        }

        [Test]
        public void Test_two_branches_can_be_created_from_same_commit()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, defaultBranch: "main", configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Commit for branch_1", sourceBranch: "branch_1")
                    .WithCommit("Commit for branch_2", sourceBranch: "branch_2", fromBranch: "main"))
                .BuildServer();

            var client = server.CreateClient();
            var repository = client.GetRepository(1);
            var commitFromBranch1 = repository.GetCommits("branch_1").FirstOrDefault();
            var commitFromBranch2 = repository.GetCommits("branch_2").FirstOrDefault();

            Assert.NotNull(commitFromBranch1);
            Assert.NotNull(commitFromBranch2);
            Assert.IsNotEmpty(commitFromBranch1.Parents);
            Assert.IsNotEmpty(commitFromBranch2.Parents);
            Assert.AreEqual(commitFromBranch1.Parents[0], commitFromBranch2.Parents[0]);
        }
    }
}
