using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ReleasesTests
    {
        [Test]
        public async Task Test_can_create_a_release()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var releasesClient = context.Client.GetRepository(project.Id).Releases;

            var release = releasesClient.Create(new ReleaseCreate
            {
                Name = "0.7",
                Ref = project.DefaultBranch,
                Description = "test",
            });
            Assert.That(release.TagName, Is.EqualTo("0.7"));
            Assert.That(release.Description, Is.EqualTo("test"));

            release = releasesClient.Update(new ReleaseUpdate() { Name = "0.7", Description = "test edited" });
            Assert.That(release.TagName, Is.EqualTo("0.7"));
            Assert.That(release.Description, Is.EqualTo("test edited"));

            releasesClient.Delete("0.7");
            Assert.IsNull(releasesClient.All.FirstOrDefault(x => string.Equals(x.TagName, "0.7", System.StringComparison.Ordinal)));
        }
    }
}
