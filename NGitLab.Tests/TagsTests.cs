using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class TagsTests
    {
        [Test]
        public async Task Test_can_tag_a_project()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var tagsClient = context.Client.GetRepository(project.Id).Tags;

            var result = tagsClient.Create(new TagCreate
            {
                Name = "v0.5",
                Message = "Test message",
                Ref = project.DefaultBranch,
                ReleaseDescription = "Test description",
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", System.StringComparison.Ordinal)));
            Assert.IsNotNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Message, "Test message", System.StringComparison.Ordinal)));

            tagsClient.Delete("v0.5");
            Assert.IsNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", System.StringComparison.Ordinal)));
        }

        [Test]
        public async Task Test_can_create_a_release()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var tagsClient = context.Client.GetRepository(project.Id).Tags;

            var result = tagsClient.Create(new TagCreate
            {
                Name = "0.7",
                Ref = project.DefaultBranch,
            });

            var release = tagsClient.CreateRelease("0.7", new ReleaseCreate() { Description = "test" });
            Assert.That(release.TagName, Is.EqualTo("0.7"));
            Assert.That(release.Description, Is.EqualTo("test"));

            release = tagsClient.UpdateRelease("0.7", new ReleaseUpdate() { Description = "test edited" });
            Assert.That(release.TagName, Is.EqualTo("0.7"));
            Assert.That(release.Description, Is.EqualTo("test edited"));

            tagsClient.Delete("0.7");
            Assert.IsNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "0.7", System.StringComparison.Ordinal)));
        }
    }
}
