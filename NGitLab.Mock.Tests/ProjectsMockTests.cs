using System.Globalization;
using System.IO;
using FluentAssertions;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class ProjectsMockTests
{
    [Test]
    public void Test_projects_created_can_be_found()
    {
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("Test", @namespace: "testgroup", addDefaultUserAsMaintainer: true)
            .BuildServer();

        var client = server.CreateClient();
        var project = client.Projects["testgroup/Test"];

        Assert.That(project, Is.Not.Null);
        Assert.That(project.Name, Is.EqualTo("Test"));
        Assert.That(project.Namespace.FullPath, Is.EqualTo("testgroup"));
    }

    [Test]
    public void Test_project_can_be_cloned_by_default()
    {
        using var tempDir = TemporaryDirectory.Create();
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("Test", clonePath: tempDir.FullPath)
            .BuildServer();

        Assert.That(Directory.Exists(tempDir.GetFullPath(".git")), Is.True);
    }

    [Test]
    public void Test_project_with_submodules()
    {
        using var tempDir = TemporaryDirectory.Create();
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("ModuleA", configure: x => x.WithCommit(configure: c => c.WithFile("A.txt")))
            .WithProject("ModuleB", configure: x => x.WithCommit(configure: c => c.WithFile("B.txt")))
            .WithProject("Test", clonePath: tempDir.FullPath, configure: x =>
                x.WithCommit("Init", configure: c
                    => c.WithSubModule("ModuleA")
                        .WithSubModule("ModuleB")))
            .BuildServer();

        Assert.That(Directory.Exists(tempDir.GetFullPath(".git")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleA/.git")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleA/A.txt")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/.git")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/B.txt")), Is.True);
    }

    [Test]
    public void Test_project_with_nested_submodules()
    {
        using var tempDir = TemporaryDirectory.Create();
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("ModuleA", configure: x => x.WithCommit(configure: c => c.WithFile("A.txt")))
            .WithProject("ModuleB", configure: x => x.WithCommit(configure: c
                                                                     => c.WithFile("B.txt")
                                                                         .WithSubModule("ModuleA")))
            .WithProject("Test", clonePath: tempDir.FullPath, configure: x =>
                x.WithCommit(configure: c
                    => c.WithSubModule("ModuleB")))
            .BuildServer();

        Assert.That(Directory.Exists(tempDir.GetFullPath(".git")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/.git")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/B.txt")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/ModuleA/.git")), Is.True);
        Assert.That(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/ModuleA/A.txt")), Is.True);
    }

    [Test]
    public void Test_projects_created_url_ends_with_namespace_and_name()
    {
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("Test", @namespace: "testgroup", addDefaultUserAsMaintainer: true)
            .BuildServer();

        var client = server.CreateClient();
        var project = client.Projects["testgroup/Test"];

        Assert.That(project, Is.Not.Null);
        Assert.That(project.SshUrl, Does.EndWith($"testgroup{Path.DirectorySeparatorChar}test"));
        Assert.That(project.HttpUrl, Does.EndWith($"testgroup{Path.DirectorySeparatorChar}test"));
        Assert.That(project.WebUrl, Does.EndWith("testgroup/test"));
    }

    [Test]
    public void Test_get_languages()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew();

        var client = server.CreateClient(user);
        Assert.That(client.Projects.GetLanguages(project.Id.ToString(CultureInfo.InvariantCulture)), Is.Empty);

        project.Repository.Commit(user, "dummy", new[] { File.CreateFromText("test.cs", "dummy"), File.CreateFromText("test.js", "dummy") });
        var languages = client.Projects.GetLanguages(project.Id.ToString(CultureInfo.InvariantCulture));
        Assert.That(languages, Has.Count.EqualTo(2));
        Assert.That(languages["C#"], Is.EqualTo(0.5d));
        Assert.That(languages["JavaScript"], Is.EqualTo(0.5d));
    }

    [Test]
    public void Test_empty_repo()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew();

        Assert.That(project.ToClientProject(user).EmptyRepo, Is.True);

        project.Repository.Commit(user, "dummy");
        Assert.That(project.ToClientProject(user).EmptyRepo, Is.False);
    }

    [Test]
    public void Test_project_permissions_maintainer_with_project_access()
    {
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("Test", @namespace: "testgroup", addDefaultUserAsMaintainer: true)
            .BuildServer();

        var client = server.CreateClient();
        var project = client.Projects["testgroup/Test"];

        project.Should().NotBeNull();
        project.Permissions.GroupAccess.Should().BeNull();
        project.Permissions.ProjectAccess.AccessLevel.Should().Be(AccessLevel.Maintainer);
    }

    [Test]
    public void Test_project_permissions_with_no_access()
    {
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithProject("Test", @namespace: "testgroup")
            .BuildServer();

        var client = server.CreateClient();
        var project = client.Projects["testgroup/Test"];

        project.Should().NotBeNull();
        project.Permissions.GroupAccess.Should().BeNull();
        project.Permissions.ProjectAccess.Should().BeNull();
    }

    [Test]
    public void Test_project_permissions_with_group_access()
    {
        using var server = new GitLabConfig()
            .WithUser("Test", isDefault: true)
            .WithGroup("testgroup", addDefaultUserAsMaintainer: true)
            .WithProject("Test", @namespace: "testgroup")
            .BuildServer();

        var client = server.CreateClient();
        var project = client.Projects["testgroup/Test"];

        project.Should().NotBeNull();
        project.Permissions.ProjectAccess.Should().BeNull();
        project.Permissions.GroupAccess.AccessLevel.Should().Be(AccessLevel.Maintainer);
    }
}
