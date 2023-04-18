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

        [Test]
        public void Test_issue_by_id_can_be_found()
        {
            using var server = new GitLabConfig()
                .WithUser("user", isDefault: true, isAdmin: true)
                .WithProject("Test", configure: project => project
                    .WithIssue("Issue title", author: "user", id: 5))
                .BuildServer();

            var client = server.CreateClient();

            var issue = client.Issues.GetById(10001);
            Assert.AreEqual(5, issue.IssueId);
            Assert.AreEqual("Issue title", issue.Title);
        }

        [Test]
        public void Test_issue_bla()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("Test", id: 1, configure: project => project
                    .WithIssue("Issue title", author: "user", id: 5)
                    .WithMilestone("Milestone 1")
                    .WithMilestone("Milestone 2"))
                .BuildServer();

            var client = server.CreateClient();
            var issuesClient = client.Issues;
            var milestone = client.GetMilestone(1).All.ToArray()[0];

            issuesClient.Edit(new IssueEdit
            {
                ProjectId = 1,
                IssueId = 5,
                MilestoneId = milestone.Id,
            });

            issuesClient.Edit(new IssueEdit
            {
                ProjectId = 1,
                IssueId = 5,
                MilestoneId = 2,
            });

            var resourceMilestoneEvents = issuesClient.ResourceMilestoneEvents(projectId: 1, issueIid: 5).ToList();
            Assert.AreEqual(3, resourceMilestoneEvents.Count);

            var removeMilestoneEvents = resourceMilestoneEvents.Where(e => e.Action == ResourceMilestoneEventAction.Remove).ToArray();
            Assert.AreEqual(1, removeMilestoneEvents.Length);
            Assert.AreEqual(1, removeMilestoneEvents[0].Milestone.Id);

            var addMilestoneEvents = resourceMilestoneEvents.Where(e => e.Action == ResourceMilestoneEventAction.Add).ToArray();
            Assert.AreEqual(2, addMilestoneEvents.Length);
            Assert.AreEqual(1, addMilestoneEvents[0].Milestone.Id);
            Assert.AreEqual(2, addMilestoneEvents[1].Milestone.Id);
        }
    }
}
