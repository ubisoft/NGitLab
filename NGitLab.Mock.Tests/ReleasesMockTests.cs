using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class ReleasesMockTests
    {
        [Test]
        public void Test_release()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithProject("Test", configure: project => project
                    .WithCommit("Changes with tag", tags: new[] { "1.2.3" })
                    .WithRelease("user1", "1.2.3"))
                .BuildServer();

            var client = server.CreateClient("user1");
            var project = client.Projects.Visible.First();
            var releaseClient = client.GetReleases(project.Id);
            var singleRelease = releaseClient.All.SingleOrDefault();

            Assert.IsNotNull(singleRelease);
            Assert.AreEqual("1.2.3", singleRelease.TagName);
            Assert.AreEqual($"{project.WebUrl}/-/releases/1.2.3", singleRelease.Links.Self);
        }
    }
}
