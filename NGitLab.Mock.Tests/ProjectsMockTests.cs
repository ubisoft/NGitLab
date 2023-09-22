using System.Globalization;
using System.IO;
using FluentAssertions;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
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

            Assert.IsNotNull(project);
            Assert.AreEqual("Test", project.Name);
            Assert.AreEqual("testgroup", project.Namespace.FullPath);
        }

        [Test]
        public void Test_project_can_be_cloned_by_default()
        {
            using var tempDir = TemporaryDirectory.Create();
            using var server = new GitLabConfig()
                .WithUser("Test", isDefault: true)
                .WithProject("Test", clonePath: tempDir.FullPath)
                .BuildServer();

            Assert.IsTrue(Directory.Exists(tempDir.GetFullPath(".git")));
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

            Assert.IsTrue(Directory.Exists(tempDir.GetFullPath(".git")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleA/.git")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleA/A.txt")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/.git")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/B.txt")));
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

            Assert.IsTrue(Directory.Exists(tempDir.GetFullPath(".git")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/.git")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/B.txt")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/ModuleA/.git")));
            Assert.IsTrue(System.IO.File.Exists(tempDir.GetFullPath("ModuleB/ModuleA/A.txt")));
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

            Assert.IsNotNull(project);
            StringAssert.EndsWith($"testgroup{Path.DirectorySeparatorChar}test", project.SshUrl);
            StringAssert.EndsWith($"testgroup{Path.DirectorySeparatorChar}test", project.HttpUrl);
            StringAssert.EndsWith("testgroup/test", project.WebUrl);
        }

        [Test]
        public void Test_get_languages()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew();

            var client = server.CreateClient(user);
            Assert.IsEmpty(client.Projects.GetLanguages(project.Id.ToString(CultureInfo.InvariantCulture)));

            project.Repository.Commit(user, "dummy", new[] { File.CreateFromText("test.cs", "dummy"), File.CreateFromText("test.js", "dummy") });
            var languages = client.Projects.GetLanguages(project.Id.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(2, languages.Count);
            Assert.AreEqual(0.5d, languages["C#"]);
            Assert.AreEqual(0.5d, languages["JavaScript"]);
        }

        [Test]
        public void Test_empty_repo()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew();

            Assert.IsTrue(project.ToClientProject(user).EmptyRepo);

            project.Repository.Commit(user, "dummy");
            Assert.IsFalse(project.ToClientProject(user).EmptyRepo);
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
}
