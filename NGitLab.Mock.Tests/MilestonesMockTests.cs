using System;
using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
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

            Assert.AreEqual(2, milestones.Length, "Milestones count is invalid");
            Assert.IsTrue(milestones.Any(x => string.Equals(x.Title, "Milestone 1", StringComparison.Ordinal)), "Milestone 'Milestone 1' not found");
            Assert.IsTrue(milestones.Any(x => string.Equals(x.Title, "Milestone 2", StringComparison.Ordinal)), "Milestone 'Milestone 2' not found");
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

            Assert.AreEqual(1, milestones.Length, "Milestones count is invalid");
            Assert.AreEqual("Milestone 1", milestones[0].Title, "Milestone 'Milestone 1' not found");
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

            Assert.AreEqual(1, milestones.Length, "Milestones count is invalid");
            Assert.AreEqual("Milestone 2", milestones[0].Title, "Milestone 'Milestone 2' not found");
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

            Assert.AreEqual(0, milestones.Length, "Milestones count is invalid");
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

            Assert.AreEqual(0, activeMilestones.Length, "Active milestones count is invalid");
            Assert.AreEqual(1, closedMilestones.Length, "Closed milestones count is invalid");

            client.GetMilestone(1).Activate(1);
            activeMilestones = client.GetMilestone(1).AllInState(Models.MilestoneState.active).ToArray();
            closedMilestones = client.GetMilestone(1).AllInState(Models.MilestoneState.closed).ToArray();

            Assert.AreEqual(1, activeMilestones.Length, "Active milestones count is invalid");
            Assert.AreEqual(0, closedMilestones.Length, "Closed milestones count is invalid");
        }

        [Test]
        public void Test_projects_merge_request_can_be_found_from_milestone()
        {
            const int ProjectId = 1;
            const int MilestoneId = 1;
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
            Assert.AreEqual(2, mergeRequests.Length, "Merge requests count is invalid");
        }

        [Test]
        public void Test_groups_merge_request_can_be_found_from_milestone()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithGroup("parentGroup", id: 1, configure: group => group
                    .WithMilestone("Milestone 1", id: 1))
                .WithGroup("subGroup1", 2, @namespace: "parentGroup")
                .WithGroup("subGroup2", 3, @namespace: "parentGroup")
                .WithProject("project1", @namespace: "parentGroup/subGroup1", addDefaultUserAsMaintainer: true, configure: project => project
                    .WithMergeRequest("branch-01", title: "Merge request 1", milestone: "Milestone 1")
                    .WithMergeRequest("branch-02", title: "Merge request 2", milestone: "Milestone 2"))
                .WithProject("project2", @namespace: "parentGroup/subGroup2", addDefaultUserAsMaintainer: true, configure: project => project
                    .WithMergeRequest("branch-03", title: "Merge request 3", milestone: "Milestone 1"))
                .BuildServer();

            var client = server.CreateClient();
            var mergeRequests = client.GetGroupMilestone(1).GetMergeRequests(1).ToArray();
            Assert.AreEqual(2, mergeRequests.Length, "Merge requests count is invalid");
        }
    }
}
