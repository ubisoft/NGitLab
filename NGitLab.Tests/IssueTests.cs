using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class IssueTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_get_issue_with_IssueQuery()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2" });

        var issues = issuesClient.Get(new IssueQuery
        {
            State = IssueState.opened,
        }).Where(i => i.ProjectId == project.Id).ToList();

        Assert.That(issues, Has.Count.EqualTo(2));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_issue_by_id()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var adminIssuesClient = context.AdminClient.Issues;

        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });

        var issue = adminIssuesClient.GetById(issue1.Id);

        Assert.That(issue.Id, Is.EqualTo(issue1.Id));
        Assert.That(issue.IssueId, Is.EqualTo(issue1.IssueId));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_unassigned_issues_with_IssueQuery()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", AssigneeId = context.Client.Users.Current.Id });

        var issues = issuesClient.Get(new IssueQuery
        {
            AssigneeId = QueryAssigneeId.None,
            State = IssueState.opened,
        }).Where(i => i.ProjectId == project.Id).ToList();

        Assert.That(issues, Has.Count.EqualTo(1));
        Assert.That(issues[0].Id, Is.EqualTo(issue1.Id));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_assigned_issues_with_IssueQuery()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", AssigneeId = context.Client.Users.Current.Id });

        var issues = issuesClient.Get(new IssueQuery
        {
            AssigneeId = context.Client.Users.Current.Id,
            State = IssueState.opened,
        }).Where(i => i.ProjectId == project.Id).ToList();

        Assert.That(issues, Has.Count.EqualTo(1));
        Assert.That(issues[0].Id, Is.EqualTo(issue2.Id));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_confidential_issues_with_IssueQuery()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1", Confidential = true });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", Confidential = true });
        var issue3 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title3" });

        var issues = issuesClient.Get(new IssueQuery
        {
            Confidential = true,
        }).Where(i => i.ProjectId == project.Id).ToList();

        Assert.That(issues, Has.Count.EqualTo(2));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_non_confidential_issues_with_IssueQuery()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1", Confidential = true });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", Confidential = true });
        var issue3 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title3" });

        var issues = issuesClient.Get(new IssueQuery
        {
            Confidential = false,
        }).Where(i => i.ProjectId == project.Id).ToList();

        Assert.That(issues, Has.Count.EqualTo(1));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_issues_no_confidential_filter_with_IssueQuery()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1", Confidential = true });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", Confidential = true });
        var issue3 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title3" });

        var issues = issuesClient.Get(new IssueQuery()).Where(i => i.ProjectId == project.Id).ToList();

        Assert.That(issues, Has.Count.EqualTo(3));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_assigned_issues_with_IssueQuery_and_project_id()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", AssigneeId = context.Client.Users.Current.Id });

        var issues = issuesClient.Get(project.Id, new IssueQuery
        {
            AssigneeId = context.Client.Users.Current.Id,
            State = IssueState.opened,
        }).ToList();

        Assert.That(issues, Has.Count.EqualTo(1));
        Assert.That(issues[0].Id, Is.EqualTo(issue2.Id));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_issues_with_invalid_project_id_will_throw()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var issuesClient = context.Client.Issues;

        Assert.Throws(Is.InstanceOf<GitLabException>(), () => issuesClient.ForProject(int.MaxValue).ToList());
        Assert.Throws(Is.InstanceOf<GitLabException>(), () => issuesClient.Get(int.MaxValue, new IssueQuery()).ToList());
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_all_project_issues()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });
        var issue2 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title2", AssigneeId = context.Client.Users.Current.Id });

        var issues = issuesClient.ForProject(project.Id).ToList();
        Assert.That(issues, Has.Count.EqualTo(2));

        issues = issuesClient.Get(project.Id, new IssueQuery()).ToList();
        Assert.That(issues, Has.Count.EqualTo(2));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_all_resource_label_events()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });

        var issues = issuesClient.ForProject(project.Id).ToList();
        Assert.That(issues, Has.Count.EqualTo(1));

        var testLabel = "test";
        var updatedIssue = issues[0];
        issuesClient.Edit(new IssueEdit
        {
            ProjectId = project.Id,
            IssueId = updatedIssue.IssueId,
            Labels = testLabel,
        });

        issuesClient.Edit(new IssueEdit
        {
            ProjectId = project.Id,
            IssueId = updatedIssue.IssueId,
            Labels = string.Empty,
        });

        var resourceLabelEvents = issuesClient.ResourceLabelEvents(project.Id, updatedIssue.IssueId).ToList();
        Assert.That(resourceLabelEvents, Has.Count.EqualTo(2));

        var addLabelEvent = resourceLabelEvents.First(e => e.Action == ResourceLabelEventAction.Add);
        Assert.That(addLabelEvent.Label.Name, Is.EqualTo(testLabel));
        Assert.That(addLabelEvent.Action, Is.EqualTo(ResourceLabelEventAction.Add));

        var removeLabelEvent = resourceLabelEvents.First(e => e.Action == ResourceLabelEventAction.Remove);
        Assert.That(removeLabelEvent.Label.Name, Is.EqualTo(testLabel));
        Assert.That(removeLabelEvent.Action, Is.EqualTo(ResourceLabelEventAction.Remove));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_all_resource_milestone_events()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1" });

        var issues = issuesClient.ForProject(project.Id).ToList();
        Assert.That(issues, Has.Count.EqualTo(1));

        var milestoneClient = context.Client.GetMilestone(project.Id);
        var milestone1 = milestoneClient.Create(new MilestoneCreate { Title = "TestMilestone", Description = "Milestone for Testing", StartDate = "2020-01-27T05:07:12.573Z", DueDate = "2020-05-26T05:07:12.573Z" });

        var updatedIssue = issues[0];
        issuesClient.Edit(new IssueEdit
        {
            ProjectId = project.Id,
            IssueId = updatedIssue.IssueId,
            MilestoneId = milestone1.Id,
        });

        issuesClient.Edit(new IssueEdit
        {
            ProjectId = project.Id,
            IssueId = updatedIssue.IssueId,
            MilestoneId = 0,
        });

        var resourceLabelEvents = issuesClient.ResourceMilestoneEvents(project.Id, updatedIssue.IssueId).ToList();
        Assert.That(resourceLabelEvents, Has.Count.EqualTo(2));

        var addMilestoneEvent = resourceLabelEvents.First(e => e.Action == ResourceMilestoneEventAction.Add);
        Assert.That(addMilestoneEvent.Milestone.Id, Is.EqualTo(milestone1.Id));
        Assert.That(addMilestoneEvent.Milestone.Title, Is.EqualTo(milestone1.Title));
        Assert.That(addMilestoneEvent.Action, Is.EqualTo(ResourceMilestoneEventAction.Add));

        var removeMilestoneEvent = resourceLabelEvents.First(e => e.Action == ResourceMilestoneEventAction.Remove);
        Assert.That(removeMilestoneEvent.Milestone.Id, Is.EqualTo(milestone1.Id));
        Assert.That(addMilestoneEvent.Milestone.Title, Is.EqualTo(milestone1.Title));
        Assert.That(removeMilestoneEvent.Action, Is.EqualTo(ResourceMilestoneEventAction.Remove));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_new_and_updated_issue_with_duedate()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;

        var initialDueDate = new DateTime(2022, 1, 1);
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1", DueDate = initialDueDate });
        var issue = issuesClient.Get(project.Id, issue1.IssueId);
        Assert.That(issue1.DueDate, Is.EqualTo(initialDueDate));

        var updatedDueDate = new DateTime(2022, 2, 1);
        var updatedIssue = issuesClient.Edit(new IssueEdit
        {
            ProjectId = project.Id,
            IssueId = issue.IssueId,
            DueDate = updatedDueDate,
        });

        Assert.That(updatedIssue.DueDate, Is.EqualTo(updatedDueDate));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_linked_issue()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;

        var issue1 = await issuesClient.CreateAsync(new IssueCreate { ProjectId = project.Id, Title = "title1" });
        var issue2 = await issuesClient.CreateAsync(new IssueCreate { ProjectId = project.Id, Title = "title2", Description = "related to #1" });
        var linked = issuesClient.CreateLinkBetweenIssues(project.Id, issue1.IssueId, project.Id, issue2.IssueId);
        Assert.That(linked, Is.True, "Expected true for create Link between issues");
        var issues = issuesClient.LinkedToAsync(project.Id, issue1.IssueId).ToList();

        // for now, no API to link issues so not links exist but API should not throw
        Assert.That(issues, Has.Count.EqualTo(1), $"Expected 1. Got {issues.Count}");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_getparticipants_issue()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issuesClient = context.Client.Issues;
        var issue1 = issuesClient.Create(new IssueCreate { ProjectId = project.Id, Title = "title1", Confidential = true });

        var participant = issuesClient.GetParticipants(project.Id, issue1.IssueId);

        Assert.That(participant.Count, Is.EqualTo(1));
    }
}
