using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

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

        Assert.That(issues, Has.Length.EqualTo(1), "Issues count is invalid");
        Assert.That(issues[0].Title, Is.EqualTo("Issue 1"), "Issue found is invalid");
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

        Assert.That(issues, Has.Length.EqualTo(1), "Issues count is invalid");
        Assert.That(issues[0].Title, Is.EqualTo("Issue 2"), "Issue found is invalid");
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
        Assert.That(issue.IssueId, Is.EqualTo(5));
        Assert.That(issue.Title, Is.EqualTo("Issue title"));
    }

    [Test]
    public void Test_issue_resource_milestone_events_can_be_found()
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
        Assert.That(resourceMilestoneEvents, Has.Count.EqualTo(3));

        var removeMilestoneEvents = resourceMilestoneEvents.Where(e => e.Action == ResourceMilestoneEventAction.Remove).ToArray();
        Assert.That(removeMilestoneEvents, Has.Length.EqualTo(1));
        Assert.That(removeMilestoneEvents[0].Milestone.Id, Is.EqualTo(1));

        var addMilestoneEvents = resourceMilestoneEvents.Where(e => e.Action == ResourceMilestoneEventAction.Add).ToArray();
        Assert.That(addMilestoneEvents, Has.Length.EqualTo(2));
        Assert.That(addMilestoneEvents[0].Milestone.Id, Is.EqualTo(1));
        Assert.That(addMilestoneEvents[1].Milestone.Id, Is.EqualTo(2));
    }
}
