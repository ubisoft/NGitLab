using System;
using System.Linq;
using NGitLab.Mock.Fluent;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class LabelsMockTests
    {
        [Test]
        public void Test_labels_can_be_found_from_project()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithProject("Test", configure: project => project
                    .WithLabel("test1")
                    .WithLabel("test2"))
                .ResolveClient();

            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(2, labels.Length, "Labels count is invalid");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), "Label test1 not found");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), "Label test2 not found");
        }

        [Test]
        public void Test_labels_can_be_added_to_project()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithProject("Test", currentAsMaintainer: true)
                .ResolveClient();

            client.Labels.Create(new LabelCreate { Id = 1, Name = "test1" });
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test1", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_edited_from_project()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithProject("Test", currentAsMaintainer: true, configure: project => project
                    .WithLabel("test1"))
                .ResolveClient();

            client.Labels.Edit(new LabelEdit { Id = 1, Name = "test1", NewName = "test2" });
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test2", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_deleted_from_project()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithProject("Test", currentAsMaintainer: true, configure: project => project
                    .WithLabel("test1"))
                .ResolveClient();

            client.Labels.Delete(new LabelDelete { Id = 1, Name = "test1" });
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(0, labels.Length, "Labels count is invalid");
        }

        [Test]
        public void Test_labels_can_be_found_from_group()
        {
            var client = new GitLabConfig()
                .WithUser("user1", isCurrent: true)
                .WithProject("Test", configure: project => project
                    .WithLabel("test1", group: true)
                    .WithLabel("test2", group: true))
                .ResolveClient();

            var labels = client.Labels.ForGroup(1).ToArray();

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
            group.Permissions.Add(new Permission(user1, AccessLevel.Maintainer));
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
            group.Permissions.Add(new Permission(user1, AccessLevel.Maintainer));
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
