using System;
using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class MilestonesMockTests
{
    [Test]
    public void Test_milestones_can_be_found_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, configure: project => project
                .WithMilestone("Milestone 1")
                .WithMilestone("Milestone 2"))
            .BuildServer();

        var client = server.CreateClient();
        var milestones = client.GetMilestone(1).All.ToArray();

        Assert.That(milestones, Has.Length.EqualTo(2), "Milestones count is invalid");
        Assert.That(milestones.Any(x => string.Equals(x.Title, "Milestone 1", StringComparison.Ordinal)), Is.True, "Milestone 'Milestone 1' not found");
        Assert.That(milestones.Any(x => string.Equals(x.Title, "Milestone 2", StringComparison.Ordinal)), Is.True, "Milestone 'Milestone 2' not found");
    }

    [Test]
    public void Test_milestones_can_be_added_to_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true)
            .BuildServer();

        var client = server.CreateClient();
        client.GetMilestone(1).Create(new MilestoneCreate { Title = "Milestone 1" });
        var milestones = client.GetMilestone(1).All.ToArray();

        Assert.That(milestones, Has.Length.EqualTo(1), "Milestones count is invalid");
        Assert.That(milestones[0].Title, Is.EqualTo("Milestone 1"), "Milestone 'Milestone 1' not found");
    }

    [Test]
    public void Test_milestones_can_be_edited_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithMilestone("Milestone 1", id: 1))
            .BuildServer();

        var client = server.CreateClient();
        client.GetMilestone(1).Update(1, new MilestoneUpdate { Title = "Milestone 2" });
        var milestones = client.GetMilestone(1).All.ToArray();

        Assert.That(milestones, Has.Length.EqualTo(1), "Milestones count is invalid");
        Assert.That(milestones[0].Title, Is.EqualTo("Milestone 2"), "Milestone 'Milestone 2' not found");
    }

    [Test]
    public void Test_milestones_can_be_deleted_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithMilestone("Milestone 1", id: 1))
            .BuildServer();

        var client = server.CreateClient();
        client.GetMilestone(1).Delete(1);
        var milestones = client.GetMilestone(1).All.ToArray();

        Assert.That(milestones, Is.Empty, "Milestones count is invalid");
    }

    [Test]
    public void Test_milestones_can_be_closed_and_activated_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithMilestone("Milestone 1", id: 1))
            .BuildServer();

        var client = server.CreateClient();
        client.GetMilestone(1).Close(1);
        var activeMilestones = client.GetMilestone(1).AllInState(Models.MilestoneState.active).ToArray();
        var closedMilestones = client.GetMilestone(1).AllInState(Models.MilestoneState.closed).ToArray();

        Assert.That(activeMilestones, Is.Empty, "Active milestones count is invalid");
        Assert.That(closedMilestones, Has.Length.EqualTo(1), "Closed milestones count is invalid");

        client.GetMilestone(1).Activate(1);
        activeMilestones = client.GetMilestone(1).AllInState(Models.MilestoneState.active).ToArray();
        closedMilestones = client.GetMilestone(1).AllInState(Models.MilestoneState.closed).ToArray();

        Assert.That(activeMilestones, Has.Length.EqualTo(1), "Active milestones count is invalid");
        Assert.That(closedMilestones, Is.Empty, "Closed milestones count is invalid");
    }

    [Test]
    public void Test_projects_merge_request_can_be_found_from_milestone()
    {
        const long ProjectId = 1;
        const long MilestoneId = 1;
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: ProjectId, addDefaultUserAsMaintainer: true, configure: project => project
                .WithMilestone("Milestone 1", id: MilestoneId)
                .WithMergeRequest("branch-01", title: "Merge request 1", milestone: "Milestone 1")
                .WithMergeRequest("branch-02", title: "Merge request 2", milestone: "Milestone 1")
                .WithMergeRequest("branch-03", title: "Merge request 3", milestone: "Milestone 2"))
            .BuildServer();

        var client = server.CreateClient();
        var mergeRequests = client.GetMilestone(ProjectId).GetMergeRequests(MilestoneId).ToArray();
        Assert.That(mergeRequests, Has.Length.EqualTo(2), "Merge requests count is invalid");
    }

    [Test]
    public void Test_groups_merge_request_can_be_found_from_milestone()
    {
        const long projectId = 1;
        const long milestoneId = 1;
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("parentGroup", id: projectId, configure: group => group
                .WithMilestone("Milestone 1", id: milestoneId))
            .WithGroup("subGroup1", 2, @namespace: "parentGroup")
            .WithGroup("subGroup2", 3, @namespace: "parentGroup")
            .WithProject("project1", @namespace: "parentGroup/subGroup1", addDefaultUserAsMaintainer: true, configure: project => project
                .WithMergeRequest("branch-01", title: "Merge request 1", milestone: "Milestone 1")
                .WithMergeRequest("branch-02", title: "Merge request 2", milestone: "Milestone 2"))
            .WithProject("project2", @namespace: "parentGroup/subGroup2", addDefaultUserAsMaintainer: true, configure: project => project
                .WithMergeRequest("branch-03", title: "Merge request 3", milestone: "Milestone 1"))
            .BuildServer();

        var client = server.CreateClient();
        var mergeRequests = client.GetGroupMilestone(projectId).GetMergeRequests(milestoneId).ToArray();
        Assert.That(mergeRequests, Has.Length.EqualTo(2), "Merge requests count is invalid");
    }
}
