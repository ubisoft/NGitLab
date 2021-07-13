using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class MilestonesMockTests
    {
        [Test]
        public void Test_milestones_can_be_found_from_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            project.Milestones.Add(new Milestone { Title = "Milestone 1" });
            project.Milestones.Add(new Milestone { Title = "Milestone 2" });

            var client = gitLabServer.CreateClient(user1);
            var milestones = client.GetMilestone(project.Id).All.ToArray();

            Assert.AreEqual(2, milestones.Length, "Milestones count is invalid");
            Assert.IsTrue(milestones.Any(x => string.Equals(x.Title, "Milestone 1", StringComparison.Ordinal)), "Milestone 'Milestone 1' not found");
            Assert.IsTrue(milestones.Any(x => string.Equals(x.Title, "Milestone 2", StringComparison.Ordinal)), "Milestone 'Milestone 2' not found");
        }

        [Test]
        public void Test_milestones_can_be_added_to_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            project.Permissions.Add(new Permission(user1, AccessLevel.Maintainer));
            group.Projects.Add(project);

            var client = gitLabServer.CreateClient(user1);
            client.GetMilestone(project.Id).Create(new MilestoneCreate { Title = "Milestone 1" });
            var milestones = client.GetMilestone(project.Id).All.ToArray();

            Assert.AreEqual(1, milestones.Length, "Milestones count is invalid");
            Assert.AreEqual("Milestone 1", milestones[0].Title, "Milestone 'Milestone 1' not found");
        }

        [Test]
        public void Test_milestones_can_be_edited_from_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            project.Permissions.Add(new Permission(user1, AccessLevel.Maintainer));
            group.Projects.Add(project);
            var milestone = new Milestone { Title = "Milestone 1" };
            project.Milestones.Add(milestone);

            var client = gitLabServer.CreateClient(user1);
            client.GetMilestone(project.Id).Update(milestone.Id, new MilestoneUpdate { Title = "Milestone 2" });
            var milestones = client.GetMilestone(project.Id).All.ToArray();

            Assert.AreEqual(1, milestones.Length, "Milestones count is invalid");
            Assert.AreEqual("Milestone 2", milestones[0].Title, "Milestone 'Milestone 2' not found");
        }

        [Test]
        public void Test_milestones_can_be_deleted_from_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            project.Permissions.Add(new Permission(user1, AccessLevel.Maintainer));
            group.Projects.Add(project);
            var milestone = new Milestone { Title = "Milestone 1" };
            project.Milestones.Add(milestone);

            var client = gitLabServer.CreateClient(user1);
            client.GetMilestone(project.Id).Delete(milestone.Id);
            var milestones = client.GetMilestone(project.Id).All.ToArray();

            Assert.AreEqual(0, milestones.Length, "Milestones count is invalid");
        }

        [Test]
        public void Test_milestones_can_be_closed_and_activated_from_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            project.Permissions.Add(new Permission(user1, AccessLevel.Maintainer));
            group.Projects.Add(project);
            var milestone = new Milestone { Title = "Milestone 1" };
            project.Milestones.Add(milestone);

            var client = gitLabServer.CreateClient(user1);
            client.GetMilestone(project.Id).Close(milestone.Id);
            var activeMilestones = client.GetMilestone(project.Id).AllInState(Models.MilestoneState.active).ToArray();
            var closedMilestones = client.GetMilestone(project.Id).AllInState(Models.MilestoneState.closed).ToArray();

            Assert.AreEqual(0, activeMilestones.Length, "Active milestones count is invalid");
            Assert.AreEqual(1, closedMilestones.Length, "Closed milestones count is invalid");

            client.GetMilestone(project.Id).Activate(milestone.Id);
            activeMilestones = client.GetMilestone(project.Id).AllInState(Models.MilestoneState.active).ToArray();
            closedMilestones = client.GetMilestone(project.Id).AllInState(Models.MilestoneState.closed).ToArray();

            Assert.AreEqual(1, activeMilestones.Length, "Active milestones count is invalid");
            Assert.AreEqual(0, closedMilestones.Length, "Closed milestones count is invalid");
        }
    }
}
