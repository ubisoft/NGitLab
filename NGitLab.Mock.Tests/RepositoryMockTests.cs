using NGitLab.Mock.Fluent;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class RepositoryMockTests
    {
        [Test]
        public void Test_project_can_be_mocked()
        {
            var client = new GitLabConfig()
                .WithUser("Test", isCurrent: true)
                .WithProject("Test", @namespace: "testgroup", currentAsMaintainer: true)
                .ResolveClient();

            var project = client.Projects["testgroup/Test"];
            Assert.IsNotNull(project);
            Assert.AreEqual("Test", project.Name);
            Assert.AreEqual("testgroup", project.Namespace.FullPath);
        }
    }
}
