using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

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
        Assert.That(tag.Protected, Is.False);

        var protectedTagClient = context.Client.GetProtectedTagClient(project.Id);
        var protectedTags = protectedTagClient.GetProtectedTagsAsync();
        Assert.That(protectedTags.Any(), Is.False);

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
        Assert.That(tagProtect.Name, Is.EqualTo(protectedTag.Name));
        var accessLevels = protectedTag.CreateAccessLevels.Select(level => level.AccessLevel).ToArray();
        Assert.That(accessLevels, Does.Contain(AccessLevel.NoAccess));
        Assert.That(accessLevels, Does.Contain(AccessLevel.Maintainer));

        var getProtectedTag = await protectedTagClient.GetProtectedTagAsync(tagProtectName).ConfigureAwait(false);
        Assert.That(getProtectedTag.Name, Is.EqualTo(protectedTag.Name));
        accessLevels = getProtectedTag.CreateAccessLevels.Select(level => level.AccessLevel).ToArray();
        Assert.That(accessLevels, Does.Contain(AccessLevel.NoAccess));
        Assert.That(accessLevels, Does.Contain(AccessLevel.Maintainer));

        var tag = await tagClient.GetByNameAsync(tagName).ConfigureAwait(false);
        Assert.That(tag.Protected, Is.True);

        await protectedTagClient.UnprotectTagAsync(tagProtectName).ConfigureAwait(false);

        var protectedTags = protectedTagClient.GetProtectedTagsAsync();
        Assert.That(protectedTags.Any(), Is.False);

        tag = await tagClient.GetByNameAsync(tag.Name).ConfigureAwait(false);
        Assert.That(tag.Protected, Is.False);
    }
}
