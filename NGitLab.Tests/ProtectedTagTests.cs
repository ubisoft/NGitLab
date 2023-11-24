using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProtectedTagTests
    {
        [Test]
        [NGitLabRetry]
        public async Task ProtectTag_Test()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var tagClient = context.Client.GetRepository(project.Id).Tags;

            var tag = tagClient.Create(new TagCreate { Name = "protectedTag", Ref = project.DefaultBranch });
            Assert.False(tag.Protected);

            var protectedTagClient = context.Client.GetProtectedTagClient(project.Id);
            var protectedTags = protectedTagClient.GetProtectedTagsAsync();
            Assert.False(protectedTags.Any());

            await ProtectedTagTest(tag.Name, tag.Name, protectedTagClient, tagClient);
            await ProtectedTagTest("*", tag.Name, protectedTagClient, tagClient);
        }

        private static async Task ProtectedTagTest(string tagProtectName, string tagName, IProtectedTagClient protectedTagClient, ITagClient tagClient)
        {
            var tagProtect = new TagProtect(tagProtectName)
            {
                AllowedToCreate = new AccessControl[] { new AccessLevelControl { AccessLevel = AccessLevel.Maintainer }, },
                CreateAccessLevel = AccessLevel.NoAccess,
            };
            var protectedTag = await protectedTagClient.ProtectTagAsync(tagProtect).ConfigureAwait(false);
            Assert.AreEqual(protectedTag.Name, tagProtect.Name);
            var accessLevels = protectedTag.CreateAccessLevels.Select(level => level.AccessLevel).ToArray();
            Assert.Contains(AccessLevel.NoAccess, accessLevels);
            Assert.Contains(AccessLevel.Maintainer, accessLevels);

            var getProtectedTag = await protectedTagClient.GetProtectedTagAsync(tagProtectName).ConfigureAwait(false);
            Assert.AreEqual(protectedTag.Name, getProtectedTag.Name);
            accessLevels = getProtectedTag.CreateAccessLevels.Select(level => level.AccessLevel).ToArray();
            Assert.Contains(AccessLevel.NoAccess, accessLevels);
            Assert.Contains(AccessLevel.Maintainer, accessLevels);

            var tag = await tagClient.GetByNameAsync(tagName).ConfigureAwait(false);
            Assert.True(tag.Protected);

            await protectedTagClient.UnprotectTagAsync(tagProtectName).ConfigureAwait(false);

            var protectedTags = protectedTagClient.GetProtectedTagsAsync();
            Assert.False(protectedTags.Any());

            tag = await tagClient.GetByNameAsync(tag.Name).ConfigureAwait(false);
            Assert.False(tag.Protected);
        }
    }
}
