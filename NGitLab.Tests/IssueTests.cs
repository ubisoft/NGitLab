using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
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

            Assert.AreEqual(2, issues.Count);
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

            Assert.AreEqual(issue1.Id, issue.Id);
            Assert.AreEqual(issue1.IssueId, issue.IssueId);
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

            Assert.AreEqual(1, issues.Count);
            Assert.AreEqual(issue1.Id, issues[0].Id);
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

            Assert.AreEqual(1, issues.Count);
            Assert.AreEqual(issue2.Id, issues[0].Id);
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

            Assert.AreEqual(2, issues.Count);
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

            Assert.AreEqual(1, issues.Count);
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

            Assert.AreEqual(3, issues.Count);
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

            Assert.AreEqual(1, issues.Count);
            Assert.AreEqual(issue2.Id, issues[0].Id);
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
            Assert.AreEqual(2, issues.Count);

            issues = issuesClient.Get(project.Id, new IssueQuery()).ToList();
            Assert.AreEqual(2, issues.Count);
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
            Assert.AreEqual(1, issues.Count);

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
            Assert.AreEqual(2, resourceLabelEvents.Count);

            var addLabelEvent = resourceLabelEvents.First(e => e.Action == ResourceLabelEventAction.Add);
            Assert.AreEqual(testLabel, addLabelEvent.Label.Name);
            Assert.AreEqual(ResourceLabelEventAction.Add, addLabelEvent.Action);

            var removeLabelEvent = resourceLabelEvents.First(e => e.Action == ResourceLabelEventAction.Remove);
            Assert.AreEqual(testLabel, removeLabelEvent.Label.Name);
            Assert.AreEqual(ResourceLabelEventAction.Remove, removeLabelEvent.Action);
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
            Assert.AreEqual(1, issues.Count);

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
            Assert.AreEqual(2, resourceLabelEvents.Count);

            var addMilestoneEvent = resourceLabelEvents.First(e => e.Action == ResourceLabelEventAction.Add);
            Assert.AreEqual(milestone1.Id, addMilestoneEvent.Milestone.Id);
            Assert.AreEqual(milestone1.Title, addMilestoneEvent.Milestone.Title);
            Assert.AreEqual(ResourceLabelEventAction.Add, addMilestoneEvent.Action);

            var removeMilestoneEvent = resourceLabelEvents.First(e => e.Action == ResourceLabelEventAction.Remove);
            Assert.AreEqual(milestone1.Id, removeMilestoneEvent.Milestone.Id);
            Assert.AreEqual(milestone1.Title, addMilestoneEvent.Milestone.Title);
            Assert.AreEqual(ResourceLabelEventAction.Remove, removeMilestoneEvent.Action);
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
            Assert.AreEqual(initialDueDate, issue1.DueDate);

            var updatedDueDate = new DateTime(2022, 2, 1);
            var updatedIssue = issuesClient.Edit(new IssueEdit
            {
                ProjectId = project.Id,
                IssueId = issue.IssueId,
                DueDate = updatedDueDate,
            });

            Assert.AreEqual(updatedDueDate, updatedIssue.DueDate);
        }
    }
}
