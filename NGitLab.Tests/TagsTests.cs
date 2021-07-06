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
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", System.StringComparison.Ordinal)));
            Assert.IsNotNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Message, "Test message", System.StringComparison.Ordinal)));

            tagsClient.Delete("v0.5");
            Assert.IsNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", System.StringComparison.Ordinal)));
        }
    }
}
