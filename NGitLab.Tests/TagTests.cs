using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class TagTests
    {
        [Test]
        [NGitLabRetry]
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
            Assert.IsNotNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", StringComparison.Ordinal)));
            Assert.IsNotNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Message, "Test message", StringComparison.Ordinal)));

            tagsClient.Delete("v0.5");
            Assert.IsNull(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", StringComparison.Ordinal)));
        }

        [NGitLabRetry]
        [TestCase("v0.5", true)]
        [TestCase("v0.6", false)]
        public async Task GetTag(string tagNameSought, bool expectExistence)
        {
            // Arrange
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var tagClient = context.Client.GetRepository(project.Id).Tags;

            var tagCreated = tagClient.Create(new TagCreate
            {
                Name = "v0.5",
                Message = "Test message",
                Ref = project.DefaultBranch,
            });
            Assert.IsNotNull(tagCreated);

            // Act/Assert
            if (expectExistence)
            {
                var tagFetched = await tagClient.GetByNameAsync(tagNameSought);
                Assert.IsNotNull(tagFetched);
            }
            else
            {
                var ex = Assert.ThrowsAsync<GitLabException>(() => tagClient.GetByNameAsync(tagNameSought));
                Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            }
        }
    }
}
