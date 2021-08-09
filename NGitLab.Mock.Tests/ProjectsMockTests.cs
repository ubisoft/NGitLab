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
                .WithUser("Test", asDefault: true)
                .WithProject("Test", @namespace: "testgroup", defaultAsMaintainer: true)
                .ResolveServer();

            var client = server.ResolveClient();
            var project = client.Projects["testgroup/Test"];

            Assert.IsNotNull(project);
            Assert.AreEqual("Test", project.Name);
            Assert.AreEqual("testgroup", project.Namespace.FullPath);
        }
    }
}
