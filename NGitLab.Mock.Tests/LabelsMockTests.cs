using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class LabelsMockTests
    {
        [Test]
        public void Test_labels_from_project_can_be_found()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            project.Labels.Add(name: "test1");
            project.Labels.Add(name: "test2");

            var client = gitLabServer.CreateClient(user1);
            var labels = client.Labels.ForProject(project.Id).ToArray();

            Assert.AreEqual(2, labels.Length, "Labels count is invalid");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), "Label test1 not found");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), "Label test2 not found");
        }

        [Test]
        public void Test_labels_can_be_added_to_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);

            var client = gitLabServer.CreateClient(user1);
            client.Labels.Create(new LabelCreate { Id = project.Id, Name = "test1" });
            var labels = client.Labels.ForProject(project.Id).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test1", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_edited_from_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            project.Labels.Add(name: "test1");

            var client = gitLabServer.CreateClient(user1);
            client.Labels.Edit(new LabelEdit { Id = project.Id, Name = "test1", NewName = "test2" });
            var labels = client.Labels.ForProject(project.Id).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test2", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_deleted_from_project()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            project.Labels.Add(name: "test1");

            var client = gitLabServer.CreateClient(user1);
            client.Labels.Delete(new LabelDelete { Id = project.Id, Name = "test1" });
            var labels = client.Labels.ForProject(project.Id).ToArray();

            Assert.AreEqual(0, labels.Length, "Labels count is invalid");
        }

        [Test]
        public void Test_labels_can_be_found_from_group()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            group.Labels.Add(name: "test1");
            group.Labels.Add(name: "test2");

            var client = gitLabServer.CreateClient(user1);
            var labels = client.Labels.ForGroup(group.Id).ToArray();

            Assert.AreEqual(2, labels.Length, "Labels count is invalid");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), "Label test1 not found");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), "Label test2 not found");
        }

        [Test]
        public void Test_labels_can_be_added_to_group()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);

            var client = gitLabServer.CreateClient(user1);
            client.Labels.CreateGroupLabel(new LabelCreate { Id = group.Id, Name = "test1" });
            var labels = client.Labels.ForGroup(group.Id).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test1", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_edited_from_group()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            group.Labels.Add(name: "test1");

            var client = gitLabServer.CreateClient(user1);
            client.Labels.EditGroupLabel(new LabelEdit { Id = group.Id, Name = "test1", NewName = "test2" });
            var labels = client.Labels.ForGroup(group.Id).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test2", labels[0].Name, "Label not found");
        }
    }
}
