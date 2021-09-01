using System.IO;
using NGitLab.Mock.Config;
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
    }
}
