using System.Linq;
using System.Threading.Tasks;
using NGitLab.Impl;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.Milestone;

public class MilestoneClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_project_milestone_api()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var milestoneClient = context.Client.GetMilestone(project.Id);

        var milestone = CreateMilestone(context, MilestoneScope.Projects, project.Id, "my-super-milestone");

        Assert.That(milestoneClient[milestone.Id].Id, Is.EqualTo(milestone.Id), "Test we can get a milestone by Id");
        Assert.That(milestoneClient.All.Any(x => x.Id == milestone.Id), Is.True, "Test 'All' accessor returns the milestone");
        Assert.That(milestoneClient.AllInState(MilestoneState.active).Any(x => x.Id == milestone.Id), Is.True, "Can return all active milestone");

        milestone = UpdateMilestone(context, MilestoneScope.Projects, project.Id, milestone);

        milestone = UpdatePartialMilestone(context, MilestoneScope.Projects, project.Id, milestone);

        milestone = CloseMilestone(context, MilestoneScope.Projects, project.Id, milestone);

        Assert.That(milestoneClient.AllInState(MilestoneState.closed).Any(x => x.Id == milestone.Id), Is.True, "Can return all closed milestone");

        milestone = ActivateMilestone(context, MilestoneScope.Projects, project.Id, milestone);

        DeleteMilestone(context, MilestoneScope.Projects, project.Id, milestone);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_group_milestone_api()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        var milestoneClient = context.Client.GetGroupMilestone(group.Id);

        var milestone = CreateMilestone(context, MilestoneScope.Groups, group.Id, "my-super-group-milestone");

        Assert.That(milestoneClient[milestone.Id].Id, Is.EqualTo(milestone.Id), "Test we can get a milestone by Id");
        Assert.That(milestoneClient.All.Any(x => x.Id == milestone.Id), Is.True, "Test 'All' accessor returns the milestone");
        Assert.That(milestoneClient.AllInState(MilestoneState.active).Any(x => x.Id == milestone.Id), Is.True, "Can return all active milestone");

        milestone = UpdateMilestone(context, MilestoneScope.Groups, group.Id, milestone);

        milestone = UpdatePartialMilestone(context, MilestoneScope.Groups, group.Id, milestone);

        milestone = CloseMilestone(context, MilestoneScope.Groups, group.Id, milestone);

        Assert.That(milestoneClient.AllInState(MilestoneState.closed).Any(x => x.Id == milestone.Id), Is.True, "Can return all closed milestone");

        milestone = ActivateMilestone(context, MilestoneScope.Groups, group.Id, milestone);

        DeleteMilestone(context, MilestoneScope.Groups, group.Id, milestone);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_project_milestone_merge_requests()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();

        var milestoneClient = context.Client.GetMilestone(project.Id);
        var milestone = CreateMilestone(context, MilestoneScope.Projects, project.Id, "my-super-milestone");

        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate { MilestoneId = milestone.Id });

        var mergeRequests = milestoneClient.GetMergeRequests(milestone.Id).ToArray();
        Assert.That(mergeRequests, Has.Length.EqualTo(1), "The query retrieved all merged requests that assigned to the milestone.");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_group_milestone_merge_requests()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        var (project, mergeRequest) = context.CreateMergeRequest(configureProject: project => project.NamespaceId = group.Id);

        var milestoneClient = context.Client.GetGroupMilestone(group.Id);
        var milestone = CreateMilestone(context, MilestoneScope.Groups, group.Id, "my-super-milestone");

        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate { MilestoneId = milestone.Id });

        var mergeRequests = milestoneClient.GetMergeRequests(milestone.Id).ToArray();
        Assert.That(mergeRequests, Has.Length.EqualTo(1), "The query retrieved all merged requests that assigned to the milestone.");
    }

    private static Models.Milestone CreateMilestone(GitLabTestContext context, MilestoneScope scope, long id, string title)
    {
        var milestoneClient = scope == MilestoneScope.Projects ? context.Client.GetMilestone(id) : context.Client.GetGroupMilestone(id);
        var milestone = milestoneClient.Create(new MilestoneCreate
        {
            Title = title,
            Description = $"{title} description",
            StartDate = "2017-08-20",
            DueDate = "2017-09-20",
        });

        Assert.That(milestone, Is.Not.Null);
        Assert.That(milestone.Title, Is.EqualTo(title));
        Assert.That(milestone.Description, Is.EqualTo($"{title} description"));
        Assert.That(milestone.StartDate, Is.EqualTo("2017-08-20"));
        Assert.That(milestone.DueDate, Is.EqualTo("2017-09-20"));
        Assert.That(milestone.ProjectId, scope == MilestoneScope.Projects ? Is.EqualTo(id) : Is.Null);
        Assert.That(milestone.GroupId, scope == MilestoneScope.Groups ? Is.EqualTo(id) : Is.Null);

        return milestone;
    }

    private static Models.Milestone UpdateMilestone(GitLabTestContext context, MilestoneScope scope, long id, Models.Milestone milestone)
    {
        var milestoneClient = scope == MilestoneScope.Projects ? context.Client.GetMilestone(id) : context.Client.GetGroupMilestone(id);
        var updatedMilestone = milestoneClient.Update(milestone.Id, new MilestoneUpdate
        {
            Title = milestone.Title + "new",
            Description = milestone.Description + "new",
            StartDate = "2018-08-20",
            DueDate = "2018-09-20",
        });

        Assert.That(updatedMilestone, Is.Not.Null);
        Assert.That(updatedMilestone.Title, Is.EqualTo(milestone.Title + "new"));
        Assert.That(updatedMilestone.Description, Is.EqualTo(milestone.Description + "new"));
        Assert.That(updatedMilestone.StartDate, Is.EqualTo("2018-08-20"));
        Assert.That(updatedMilestone.DueDate, Is.EqualTo("2018-09-20"));
        Assert.That(updatedMilestone.State, Is.EqualTo(milestone.State));
        Assert.That(milestone.ProjectId, scope == MilestoneScope.Projects ? Is.EqualTo(id) : Is.Null);
        Assert.That(milestone.GroupId, scope == MilestoneScope.Groups ? Is.EqualTo(id) : Is.Null);

        return updatedMilestone;
    }

    private static Models.Milestone UpdatePartialMilestone(GitLabTestContext context, MilestoneScope scope, long id, Models.Milestone milestone)
    {
        var milestoneClient = scope == MilestoneScope.Projects ? context.Client.GetMilestone(id) : context.Client.GetGroupMilestone(id);
        var updatedMilestone = milestoneClient.Update(milestone.Id, new MilestoneUpdate
        {
            Description = milestone.Description + "partial new",
        });

        Assert.That(updatedMilestone, Is.Not.Null);
        Assert.That(updatedMilestone.Title, Is.EqualTo(milestone.Title));
        Assert.That(updatedMilestone.Description, Is.EqualTo(milestone.Description + "partial new"));
        Assert.That(updatedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
        Assert.That(updatedMilestone.DueDate, Is.EqualTo(milestone.DueDate));
        Assert.That(updatedMilestone.State, Is.EqualTo(milestone.State));
        Assert.That(updatedMilestone.ProjectId, scope == MilestoneScope.Projects ? Is.EqualTo(id) : Is.Null);
        Assert.That(updatedMilestone.GroupId, scope == MilestoneScope.Groups ? Is.EqualTo(id) : Is.Null);

        return updatedMilestone;
    }

    private static Models.Milestone ActivateMilestone(GitLabTestContext context, MilestoneScope scope, long id, Models.Milestone milestone)
    {
        var milestoneClient = scope == MilestoneScope.Projects ? context.Client.GetMilestone(id) : context.Client.GetGroupMilestone(id);
        var activeMilestone = milestoneClient.Activate(milestone.Id);

        Assert.That(activeMilestone.State, Is.EqualTo(nameof(MilestoneState.active)));
        Assert.That(activeMilestone.Title, Is.EqualTo(milestone.Title));
        Assert.That(activeMilestone.Description, Is.EqualTo(milestone.Description));
        Assert.That(activeMilestone.StartDate, Is.EqualTo(milestone.StartDate));
        Assert.That(activeMilestone.DueDate, Is.EqualTo(milestone.DueDate));
        Assert.That(activeMilestone.ProjectId, scope == MilestoneScope.Projects ? Is.EqualTo(id) : Is.Null);
        Assert.That(activeMilestone.GroupId, scope == MilestoneScope.Groups ? Is.EqualTo(id) : Is.Null);

        return activeMilestone;
    }

    private static Models.Milestone CloseMilestone(GitLabTestContext context, MilestoneScope scope, long id, Models.Milestone milestone)
    {
        var milestoneClient = scope == MilestoneScope.Projects ? context.Client.GetMilestone(id) : context.Client.GetGroupMilestone(id);
        var closedMilestone = milestoneClient.Close(milestone.Id);

        Assert.That(closedMilestone.State, Is.EqualTo(nameof(MilestoneState.closed)));
        Assert.That(closedMilestone.Title, Is.EqualTo(milestone.Title));
        Assert.That(closedMilestone.Description, Is.EqualTo(milestone.Description));
        Assert.That(closedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
        Assert.That(closedMilestone.DueDate, Is.EqualTo(milestone.DueDate));
        Assert.That(closedMilestone.ProjectId, scope == MilestoneScope.Projects ? Is.EqualTo(id) : Is.Null);
        Assert.That(closedMilestone.GroupId, scope == MilestoneScope.Groups ? Is.EqualTo(id) : Is.Null);

        return closedMilestone;
    }

    private static void DeleteMilestone(GitLabTestContext context, MilestoneScope scope, long id, Models.Milestone milestone)
    {
        var milestoneClient = scope == MilestoneScope.Projects ? context.Client.GetMilestone(id) : context.Client.GetGroupMilestone(id);
        milestoneClient.Delete(milestone.Id);

        Assert.Throws<GitLabException>(() =>
        {
            var ms = milestoneClient[milestone.Id];
        });
    }
}
