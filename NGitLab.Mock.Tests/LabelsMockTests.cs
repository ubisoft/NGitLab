using System;
using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class LabelsMockTests
{
    [Test]
    public void Test_labels_can_be_found_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, configure: project => project
                .WithLabel("test1")
                .WithLabel("test2"))
            .BuildServer();

        var client = server.CreateClient();
        var labels = client.Labels.ForProject(1).ToArray();

        Assert.That(labels, Has.Length.EqualTo(2), "Labels count is invalid");
        Assert.That(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), Is.True, "Label test1 not found");
        Assert.That(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), Is.True, "Label test2 not found");
    }

    [Test]
    public void Test_labels_can_be_added_to_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true)
            .BuildServer();

        var client = server.CreateClient();
        client.Labels.CreateProjectLabel(1, new ProjectLabelCreate { Name = "test1" });
        var labels = client.Labels.ForProject(1).ToArray();

        Assert.That(labels, Has.Length.EqualTo(1), "Labels count is invalid");
        Assert.That(labels[0].Name, Is.EqualTo("test1"), "Label not found");
    }

    [Test]
    public void Test_labels_can_be_edited_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithLabel("test1"))
            .BuildServer();

        var client = server.CreateClient();
        client.Labels.EditProjectLabel(1, new ProjectLabelEdit { Name = "test1", NewName = "test2" });
        var labels = client.Labels.ForProject(1).ToArray();

        Assert.That(labels, Has.Length.EqualTo(1), "Labels count is invalid");
        Assert.That(labels[0].Name, Is.EqualTo("test2"), "Label not found");
    }

    [Test]
    public void Test_labels_can_be_deleted_from_project()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithLabel("test1"))
            .BuildServer();

        var client = server.CreateClient();
        client.Labels.DeleteProjectLabel(1, new ProjectLabelDelete { Name = "test1" });
        var labels = client.Labels.ForProject(1).ToArray();

        Assert.That(labels, Is.Empty, "Labels count is invalid");
    }

    [Test]
    public void Test_labels_can_be_found_from_group()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("Test", id: 2, configure: project => project
                .WithLabel("test1")
                .WithLabel("test2"))
            .BuildServer();

        var client = server.CreateClient();
        var labels = client.Labels.ForGroup(2).ToArray();

        Assert.That(labels, Has.Length.EqualTo(2), "Labels count is invalid");
        Assert.That(labels.Any(x => string.Equals(x.Name, "test1", StringComparison.Ordinal)), Is.True, "Label test1 not found");
        Assert.That(labels.Any(x => string.Equals(x.Name, "test2", StringComparison.Ordinal)), Is.True, "Label test2 not found");
    }

    [Test]
    public void Test_labels_can_be_added_to_group()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("Test", id: 2, addDefaultUserAsMaintainer: true)
            .BuildServer();

        var client = server.CreateClient();
        client.Labels.CreateGroupLabel(2, new GroupLabelCreate { Name = "test1" });
        var labels = client.Labels.ForGroup(2).ToArray();

        Assert.That(labels, Has.Length.EqualTo(1), "Labels count is invalid");
        Assert.That(labels[0].Name, Is.EqualTo("test1"), "Label not found");
    }

    [Test]
    public void Test_labels_can_be_edited_from_group()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("Test", id: 2, addDefaultUserAsMaintainer: true, configure: project => project
                .WithLabel("test1"))
            .BuildServer();

        var client = server.CreateClient();
        client.Labels.EditGroupLabel(2, new GroupLabelEdit { Name = "test1", NewName = "test2" });
        var labels = client.Labels.ForGroup(2).ToArray();

        Assert.That(labels, Has.Length.EqualTo(1), "Labels count is invalid");
        Assert.That(labels[0].Name, Is.EqualTo("test2"), "Label not found");
    }
}
