using System.Linq;
using NGitLab.Mock.Fluent;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class IssuesMockTests
    {
        [Test]
        public void Test_issues_created_by_me_can_be_listed()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue 1", author: "user1", assignee: "user2")
                    .WithIssue("Issue 2", author: "user2", assignee: "user1"))
                .ResolveClient();

            var issues = client.Issues.Get(new IssueQuery { Scope = "created_by_me" }).ToArray();

            Assert.AreEqual(1, issues.Length, "Issues count is invalid");
            Assert.AreEqual("Issue 1", issues[0].Title, "Issue found is invalid");
        }

        [Test]
        public void Test_issues_assigned_to_me_can_be_listed()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue 1", author: "user1", assignee: "user2")
                    .WithIssue("Issue 2", author: "user2", assignee: "user1"))
                .ResolveClient();

            var issues = client.Issues.Get(new IssueQuery { Scope = "assigned_to_me" }).ToArray();

            Assert.AreEqual(1, issues.Length, "Issues count is invalid");
            Assert.AreEqual("Issue 2", issues[0].Title, "Issue found is invalid");
        }

        [Test]
        public void Test_issues_assignee_not_throwing_when_assignees_is_null()
        {
            var client = new GitLabConfig()
                .WithUser("user", isCurrent: true)
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue title", author: "user"))
                .ResolveClient();

            Assert.DoesNotThrow(() => client.Issues.Get(new IssueQuery { Scope = "assigned_to_me" }).ToArray());
        }
    }
}
