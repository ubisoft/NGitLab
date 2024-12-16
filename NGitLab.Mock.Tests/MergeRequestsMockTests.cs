using System;
using System.Linq;
using System.Net;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class MergeRequestsMockTests
{
    [Test]
    public void Test_merge_requests_created_by_me_can_be_listed()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test", configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2")
                .WithMergeRequest("branch-02", title: "Merge request 2", author: "user2", assignee: "user1"))
            .BuildServer();

        var client = server.CreateClient("user1");
        var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { Scope = "created_by_me" }).ToArray();

        Assert.That(mergeRequests, Has.Length.EqualTo(1), "Merge requests count is invalid");
        Assert.That(mergeRequests[0].Title, Is.EqualTo("Merge request 1"), "Merge request found is invalid");
    }

    [Test]
    public void Test_merge_requests_assigned_to_me_can_be_listed()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test", configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2")
                .WithMergeRequest("branch-02", title: "Merge request 2", author: "user2", assignee: "user1"))
            .BuildServer();

        var client = server.CreateClient("user1");
        var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { Scope = "assigned_to_me" }).ToArray();

        Assert.That(mergeRequests, Has.Length.EqualTo(1), "Merge requests count is invalid");
        Assert.That(mergeRequests[0].Title, Is.EqualTo("Merge request 2"), "Merge request found is invalid");
    }

    [Test]
    public void Test_merge_requests_approvable_by_me_can_be_listed()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test", configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", approvers: new[] { "user2" })
                .WithMergeRequest("branch-02", title: "Merge request 2", author: "user2", approvers: new[] { "user1" }))
            .BuildServer();

        var client = server.CreateClient("user1");
        var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { ApproverIds = [1L] }).ToArray();

        Assert.That(mergeRequests, Has.Length.EqualTo(1), "Merge requests count is invalid");
        Assert.That(mergeRequests[0].Title, Is.EqualTo("Merge request 2"), "Merge request found is invalid");
    }

    [Test]
    public void Test_merge_requests_can_be_listed_when_assignee_not_set()
    {
        using var gitLabServer = new GitLabServer();
        var user1 = new User("user1");
        gitLabServer.Users.Add(user1);
        var user2 = new User("user2");
        gitLabServer.Users.Add(user2);
        var group = new Group("TestGroup");
        gitLabServer.Groups.Add(group);
        var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
        group.Projects.Add(project);
        var mergeRequest1 = new MergeRequest { Author = new UserRef(user1), Title = "Merge request 1", SourceProject = project };
        project.MergeRequests.Add(mergeRequest1);
        var mergeRequest2 = new MergeRequest { Author = new UserRef(user2), Assignee = new UserRef(user1), Title = "Merge request 2", SourceProject = project };
        project.MergeRequests.Add(mergeRequest2);

        var client = gitLabServer.CreateClient(user1);
        var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { Scope = "assigned_to_me" }).ToArray();

        Assert.That(mergeRequests, Has.Length.EqualTo(1), "Merge requests count is invalid");
        Assert.That(mergeRequests[0].Title, Is.EqualTo("Merge request 2"), "Merge request found is invalid");
    }

    [Test]
    public void Test_merge_requests_assignee_should_update_assignees_and_vice_versa()
    {
        var user1 = new User("user1");
        var user2 = new User("user2");

        var mergeRequestSingle = new MergeRequest
        {
            Assignee = new UserRef(user1),
        };

        var mergeRequestTwo = new MergeRequest
        {
            Assignees = new[] { new UserRef(user1), new UserRef(user2) },
        };

        Assert.That(mergeRequestSingle.Assignees, Has.Count.EqualTo(1), "Merge request assignees count invalid");
        Assert.That(mergeRequestTwo.Assignee.UserName, Is.EqualTo("user1"), "Merge request assignee is invalid");
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Test_merge_request_with_no_rebase_required_can_be_accepted(bool sourceProjectSameAsTargetProject)
    {
        // Arrange
        using var server = new GitLabServer();

        var contributor = server.Users.AddNew("contributor");
        var maintainer = server.Users.AddNew("maintainer");

        var targetGroup = new Group("TheTargetGroup");
        server.Groups.Add(targetGroup);

        var targetProject = new Project("TheTargetProject") { Visibility = VisibilityLevel.Internal };
        targetGroup.Projects.Add(targetProject);

        targetProject.Permissions.Add(new Permission(maintainer, AccessLevel.Maintainer));
        targetProject.Repository.Commit(maintainer, "A commit");

        var sourceProject = sourceProjectSameAsTargetProject ?
            targetProject :
            targetProject.Fork(contributor.Namespace, contributor, "TheSourceProject");
        sourceProject.Repository.CreateAndCheckoutBranch("to-be-merged");
        sourceProject.Repository.Commit(contributor, "add a file", new[] { File.CreateFromText(Guid.NewGuid().ToString("N"), "This is the new file's content") });

        var mr = targetProject.CreateMergeRequest(contributor, "A great title", "A great description", targetProject.DefaultBranch, "to-be-merged", sourceProject);
        mr.Assignee = new UserRef(maintainer);

        targetProject.MergeMethod = "ff";

        var maintainerClient = server.CreateClient("maintainer");

        // Act
        var modelMr = maintainerClient.GetMergeRequest(mr.Project.Id).Accept(mr.Iid, new MergeRequestMerge
        {
            MergeWhenPipelineSucceeds = mr.HeadPipeline != null,
            ShouldRemoveSourceBranch = true,
            Sha = mr.HeadSha,
        });

        // Assert
        Assert.That(modelMr.HasConflicts, Is.False);
        Assert.That(modelMr.DivergedCommitsCount, Is.EqualTo(0));
        Assert.That(modelMr.DiffRefs?.BaseSha, Is.Not.Null);
        Assert.That(modelMr.DiffRefs.StartSha, Is.EqualTo(modelMr.DiffRefs.BaseSha));
        Assert.That(modelMr.State, Is.EqualTo("merged"));

        Assert.That(targetProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)), Is.False,
            "Since the merge succeeded and 'ShouldRemoveSourceBranch' was set, 'to-be-merged' branch should be gone");

        Assert.That(sourceProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)), Is.False,
            "Since the merge succeeded and 'ShouldRemoveSourceBranch' was set, 'to-be-merged' branch should be gone");
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Test_merge_request_with_non_conflicting_rebase_needed_and_merge_method_ff_cannot_be_accepted(bool sourceProjectSameAsTargetProject)
    {
        // Arrange
        using var server = new GitLabServer();

        var contributor = server.Users.AddNew("contributor");
        var maintainer = server.Users.AddNew("maintainer");

        var targetGroup = new Group("TheTargetGroup");
        server.Groups.Add(targetGroup);

        var targetProject = new Project("TheTargetProject") { Visibility = VisibilityLevel.Internal };
        targetGroup.Projects.Add(targetProject);

        targetProject.Permissions.Add(new Permission(maintainer, AccessLevel.Maintainer));
        targetProject.Repository.Commit(maintainer, "A commit");

        var sourceProject = sourceProjectSameAsTargetProject ?
            targetProject :
            targetProject.Fork(contributor.Namespace, contributor, "TheSourceProject");
        sourceProject.Repository.CreateAndCheckoutBranch("to-be-merged");
        sourceProject.Repository.Commit(contributor, "add a file", new[] { File.CreateFromText(Guid.NewGuid().ToString("N"), "This is the new file's content") });

        targetProject.MergeMethod = "ff";

        targetProject.Repository.Checkout(targetProject.DefaultBranch);
        targetProject.Repository.Commit(maintainer, "add a file", new[] { File.CreateFromText(Guid.NewGuid().ToString("N"), "This is the new file's content") });

        var mr = targetProject.CreateMergeRequest(contributor, "A great title", "A great description", targetProject.DefaultBranch, "to-be-merged", sourceProject);
        mr.Assignee = new UserRef(maintainer);

        var maintainerClient = server.CreateClient("maintainer");

        // Act/Assert
        var exception = Assert.Throws<GitLabException>(() => maintainerClient.GetMergeRequest(mr.Project.Id).Accept(mr.Iid, new MergeRequestMerge
        {
            MergeWhenPipelineSucceeds = mr.HeadPipeline != null,
            ShouldRemoveSourceBranch = true,
        }));
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        Assert.That(exception.Message.Equals("The MR cannot be merged with method 'ff': the source branch must first be rebased", StringComparison.Ordinal), Is.True);

        Assert.That(mr.HasConflicts, Is.False);
        Assert.That(mr.DivergedCommitsCount, Is.EqualTo(1));
        Assert.That(mr.StartSha, Is.Not.EqualTo(mr.BaseSha));

        Assert.That(targetProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)), Is.True,
            "Since the merge failed, 'to-be-merged' branch should still be there");

        Assert.That(sourceProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)), Is.True,
            "Since the merge failed, 'to-be-merged' branch should still be there");
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Test_merge_request_with_conflicts_cannot_be_accepted(bool sourceProjectSameAsTargetProject)
    {
        // Arrange
        using var server = new GitLabServer();

        var contributor = server.Users.AddNew("contributor");
        var maintainer = server.Users.AddNew("maintainer");

        var targetGroup = new Group("TheTargetGroup");
        server.Groups.Add(targetGroup);

        var targetProject = new Project("TheTargetProject") { Visibility = VisibilityLevel.Internal };
        targetGroup.Projects.Add(targetProject);

        targetProject.Permissions.Add(new Permission(maintainer, AccessLevel.Maintainer));
        targetProject.Repository.Commit(maintainer, "A commit");

        var sourceProject = sourceProjectSameAsTargetProject ?
            targetProject :
            targetProject.Fork(contributor.Namespace, contributor, "TheSourceProject");
        var conflictingFile = Guid.NewGuid().ToString("N");
        sourceProject.Repository.CreateAndCheckoutBranch("to-be-merged");
        sourceProject.Repository.Commit(contributor, "add a file", new[] { File.CreateFromText(conflictingFile, "This is the new file's content") });

        targetProject.MergeMethod = "ff";

        targetProject.Repository.Checkout(targetProject.DefaultBranch);
        targetProject.Repository.Commit(maintainer, "add a file", new[] { File.CreateFromText(conflictingFile, "This is conflicting content") });

        var mr = targetProject.CreateMergeRequest(contributor, "A great title", "A great description", targetProject.DefaultBranch, "to-be-merged", sourceProject);
        mr.Assignee = new UserRef(maintainer);

        var maintainerClient = server.CreateClient("maintainer");

        // Act/Assert
        var exception = Assert.Throws<GitLabException>(() => maintainerClient.GetMergeRequest(mr.Project.Id).Accept(mr.Iid, new MergeRequestMerge
        {
            MergeWhenPipelineSucceeds = mr.HeadPipeline != null,
            ShouldRemoveSourceBranch = true,
        }));
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotAcceptable));
        Assert.That(exception.Message.Equals("The merge request has some conflicts and cannot be merged", StringComparison.Ordinal), Is.True);

        Assert.That(mr.HasConflicts, Is.True);
        Assert.That(mr.DivergedCommitsCount, Is.EqualTo(1));
        Assert.That(mr.StartSha, Is.Not.EqualTo(mr.BaseSha));

        Assert.That(targetProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)), Is.True,
            "Since the merge failed, 'to-be-merged' branch should still be there");

        Assert.That(sourceProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)), Is.True,
            "Since the merge failed, 'to-be-merged' branch should still be there");
    }

    [Test]
    public void Test_merge_request_with_head_pipeline()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var commit = project.Repository.Commit(user, "test");

        var branch = "my-branch";
        project.Repository.CreateAndCheckoutBranch(branch);
        commit = project.Repository.Commit(user, "another test");

        var mr = project.CreateMergeRequest(user, "A great title", "A great description", project.DefaultBranch, branch);
        Assert.That(mr.HeadPipeline, Is.Null, "No pipeline created yet on the source branch");

        var pipeline = project.Pipelines.Add(branch, JobStatus.Success, user);
        Assert.That(mr.HeadPipeline, Is.EqualTo(pipeline), "A pipeline was just created on the source branch");
    }

    [Test]
    public void Test_merge_request_resource_state_events_found_on_close_and_reopen()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test", configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2"))
            .BuildServer();

        var client = server.CreateClient("user1");
        var projectId = server.AllProjects.First().Id;
        var mrClient = client.GetMergeRequest(projectId);
        var mergeRequest = mrClient.Get(new MergeRequestQuery { Scope = "created_by_me" }).First();

        mrClient.Close(mergeRequest.Iid);
        mrClient.Reopen(mergeRequest.Iid);

        var resourceStateEvents = mrClient.ResourceStateEventsAsync(projectId: projectId, mergeRequestIid: mergeRequest.Iid).ToArray();
        Assert.That(resourceStateEvents, Has.Length.EqualTo(2));

        var closeStateEvents = resourceStateEvents.Where(e => string.Equals(e.State, "closed", StringComparison.Ordinal)).ToArray();
        Assert.That(closeStateEvents, Has.Length.EqualTo(1));

        var reopenMilestoneEvents = resourceStateEvents.Where(e => string.Equals(e.State, "reopened", StringComparison.Ordinal)).ToArray();
        Assert.That(reopenMilestoneEvents, Has.Length.EqualTo(1));
    }

    [Test]
    public void Test_merge_request_resource_label_events_found()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test", configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2"))
            .BuildServer();

        var client = server.CreateClient("user1");
        var projectId = server.AllProjects.First().Id;
        var mrClient = client.GetMergeRequest(projectId);
        var mergeRequest = mrClient.Get(new MergeRequestQuery { Scope = "created_by_me" }).First();

        mrClient.Update(mergeRequest.Iid, new MergeRequestUpdate()
        {
            AddLabels = "first,second,third",
        });

        mrClient.Update(mergeRequest.Iid, new MergeRequestUpdate()
        {
            RemoveLabels = "second",
        });

        mrClient.Update(mergeRequest.Iid, new MergeRequestUpdate()
        {
            Labels = "first,second",
        });

        /* We're expecting this sequence
         * 1. Add first
         * 1. Add second
         * 1. Add third
         * 2. Remove second
         * 3. Add second
         * 3. Remove third
         */
        var resourceLabelEvents = mrClient.ResourceLabelEventsAsync(projectId: projectId, mergeRequestIid: mergeRequest.Iid).ToArray();
        Assert.That(resourceLabelEvents, Has.Length.EqualTo(6));

        var addLabelEvents = resourceLabelEvents.Where(e => e.Action == ResourceLabelEventAction.Add).ToArray();
        Assert.That(addLabelEvents, Has.Length.EqualTo(4));

        var removeLabelEvents = resourceLabelEvents.Where(e => e.Action == ResourceLabelEventAction.Remove).ToArray();
        Assert.That(removeLabelEvents, Has.Length.EqualTo(2));
    }

    [Test]
    public void Test_merge_request_resource_milestone_events_found()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test", configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2")
                .WithMilestone("Milestone 1")
                .WithMilestone("Milestone 2"))
            .BuildServer();

        var client = server.CreateClient("user1");
        var projectId = server.AllProjects.First().Id;
        var mrClient = client.GetMergeRequest(projectId);
        var mergeRequest = mrClient.Get(new MergeRequestQuery { Scope = "created_by_me" }).First();
        var milestones = client.GetMilestone(1).All.ToArray();

        mrClient.Update(mergeRequest.Iid, new MergeRequestUpdate()
        {
            MilestoneId = milestones[0].Id,
        });

        mrClient.Update(mergeRequest.Iid, new MergeRequestUpdate()
        {
            MilestoneId = milestones[1].Id,
        });

        /* We're expecting this sequence
         * 1. Add milestone 1
         * 2. Remove milestone 1
         * 2. Add milestone 2
         */
        var resourceMilestoneEvents = mrClient.ResourceMilestoneEventsAsync(projectId: projectId, mergeRequestIid: mergeRequest.Iid).ToArray();
        Assert.That(resourceMilestoneEvents, Has.Length.EqualTo(3));

        var removeMilestoneEvents = resourceMilestoneEvents.Where(e => e.Action == ResourceMilestoneEventAction.Remove).ToArray();
        Assert.That(removeMilestoneEvents, Has.Length.EqualTo(1));
        Assert.That(removeMilestoneEvents[0].Milestone.Id, Is.EqualTo(milestones[0].Id));

        var addMilestoneEvents = resourceMilestoneEvents.Where(e => e.Action == ResourceMilestoneEventAction.Add).ToArray();
        Assert.That(addMilestoneEvents, Has.Length.EqualTo(2));
        Assert.That(addMilestoneEvents[0].Milestone.Id, Is.EqualTo(milestones[0].Id));
        Assert.That(addMilestoneEvents[1].Milestone.Id, Is.EqualTo(milestones[1].Id));
    }

    [Test]
    public void Test_create_merge_request_without_target_project_id()
    {
        using var server = new GitLabConfig()
            .WithUser("User1", isDefault: true)
            .WithProject("Test", addDefaultUserAsMaintainer: true, configure: project => project
                .WithCommit("Initial commit")
                .WithCommit("Second commit", sourceBranch: "feature-branch", configure: commit => commit
                    .WithFile("file.txt", "content")))
            .BuildServer();

        var client = server.CreateClient();

        var mergeRequest = client
            .GetMergeRequest(1)
            .Create(new MergeRequestCreate
            {
                SourceBranch = "feature-branch",
                TargetBranch = "main",
                Title = "Merge request 1",
            });

        Assert.That(mergeRequest, Is.Not.Null);
    }
}
