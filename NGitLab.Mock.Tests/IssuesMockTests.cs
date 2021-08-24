using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class IssuesMockTests
    {
        [Test]
        public void Test_issues_created_by_me_can_be_listed()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue 1", author: "user1", assignee: "user2")
                    .WithIssue("Issue 2", author: "user2", assignee: "user1"))
                .BuildServer();

            var client = server.CreateClient("user1");
            var issues = client.Issues.Get(new IssueQuery { Scope = "created_by_me" }).ToArray();

            Assert.AreEqual(1, issues.Length, "Issues count is invalid");
            Assert.AreEqual("Issue 1", issues[0].Title, "Issue found is invalid");
        }

        [Test]
        public void Test_issues_assigned_to_me_can_be_listed()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue 1", author: "user1", assignee: "user2")
                    .WithIssue("Issue 2", author: "user2", assignee: "user1"))
                .BuildServer();

            var client = server.CreateClient("user1");
            var issues = client.Issues.Get(new IssueQuery { Scope = "assigned_to_me" }).ToArray();

            Assert.AreEqual(1, issues.Length, "Issues count is invalid");
            Assert.AreEqual("Issue 2", issues[0].Title, "Issue found is invalid");
        }

        [Test]
        public void Test_issues_assignee_not_throwing_when_assignees_is_null()
        {
            using var server = new GitLabConfig()
                .WithUser("user", isDefault: true)
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue title", author: "user"))
                .BuildServer();

            var client = server.CreateClient();
            Assert.DoesNotThrow(() => client.Issues.Get(new IssueQuery { Scope = "assigned_to_me" }).ToArray());
        }
    }
}
