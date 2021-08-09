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
                .WithUser("user1", asDefault: true)
                .WithProject("test-project", id: 1, configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Create branch", sourceBranch: "branch-01"))
                .ResolveServer();

            var client = server.ResolveClient();
            var commit = client.GetCommits(1).GetCommit("branch-01");

            Assert.AreEqual("Create branch", commit.Message.TrimEnd('\r', '\n'));
        }

        [Test]
        public void Test_commits_with_tags_can_be_found()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("test-project", id: 1, configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Changes with tag", tags: new[] { "1.0.0" }))
                .ResolveServer();

            var client = server.ResolveClient();
            var commit = client.GetCommits(1).GetCommit("1.0.0");

            Assert.AreEqual("Changes with tag", commit.Message.TrimEnd('\r', '\n'));
        }

        [Test]
        public void Test_tags_from_commit_can_be_found()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("test-project", id: 1, defaultAsMaintainer: true, configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Changes with tag", tags: new[] { "1.0.0" }))
                .ResolveServer();

            var client = server.ResolveClient();
            var tags = client.GetRepository(1).Tags.All.ToArray();

            Assert.That(tags, Has.One.Items);
            Assert.AreEqual("1.0.0", tags[0].Name);
        }
    }
}
