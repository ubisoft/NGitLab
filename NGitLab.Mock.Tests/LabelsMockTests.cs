using System;
using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class LabelsMockTests
    {
        [Test]
        public void Test_labels_can_be_found_from_project()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("Test", id: 1, configure: project => project
                    .WithLabel("test1")
                    .WithLabel("test2"))
                .ResolveServer();

            var client = server.ResolveClient();
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(2, labels.Length, "Labels count is invalid");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), "Label test1 not found");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), "Label test2 not found");
        }

        [Test]
        public void Test_labels_can_be_added_to_project()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("Test", id: 1, defaultAsMaintainer: true)
                .ResolveServer();

            var client = server.ResolveClient();
            client.Labels.Create(new LabelCreate { Id = 1, Name = "test1" });
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test1", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_edited_from_project()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("Test", id: 1, defaultAsMaintainer: true, configure: project => project
                    .WithLabel("test1"))
                .ResolveServer();

            var client = server.ResolveClient();
            client.Labels.Edit(new LabelEdit { Id = 1, Name = "test1", NewName = "test2" });
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test2", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_deleted_from_project()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("Test", id: 1, defaultAsMaintainer: true, configure: project => project
                    .WithLabel("test1"))
                .ResolveServer();

            var client = server.ResolveClient();
            client.Labels.Delete(new LabelDelete { Id = 1, Name = "test1" });
            var labels = client.Labels.ForProject(1).ToArray();

            Assert.AreEqual(0, labels.Length, "Labels count is invalid");
        }

        [Test]
        public void Test_labels_can_be_found_from_group()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithGroup("Test", id: 2, configure: project => project
                    .WithLabel("test1")
                    .WithLabel("test2"))
                .ResolveServer();

            var client = server.ResolveClient();
            var labels = client.Labels.ForGroup(2).ToArray();

            Assert.AreEqual(2, labels.Length, "Labels count is invalid");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), "Label test1 not found");
            Assert.IsTrue(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), "Label test2 not found");
        }

        [Test]
        public void Test_labels_can_be_added_to_group()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithGroup("Test", id: 2, defaultAsMaintainer: true)
                .ResolveServer();

            var client = server.ResolveClient();
            client.Labels.CreateGroupLabel(new LabelCreate { Id = 2, Name = "test1" });
            var labels = client.Labels.ForGroup(2).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test1", labels[0].Name, "Label not found");
        }

        [Test]
        public void Test_labels_can_be_edited_from_group()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithGroup("Test", id: 2, defaultAsMaintainer: true, configure: project => project
                    .WithLabel("test1"))
                .ResolveServer();

            var client = server.ResolveClient();
            client.Labels.EditGroupLabel(new LabelEdit { Id = 2, Name = "test1", NewName = "test2" });
            var labels = client.Labels.ForGroup(2).ToArray();

            Assert.AreEqual(1, labels.Length, "Labels count is invalid");
            Assert.AreEqual("test2", labels[0].Name, "Label not found");
        }
    }
}
