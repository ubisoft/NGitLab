using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.Release;

public class ReleaseClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_release_api()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var releaseClient = context.Client.GetReleases(project.Id);
        var tagsClient = context.Client.GetRepository(project.Id).Tags;

        var tag = tagsClient.Create(new TagCreate
        {
            Name = "0.7",
            Ref = project.DefaultBranch,
        });

        var release = releaseClient.Create(new ReleaseCreate
        {
            TagName = tag.Name,
            Description = "test",
        });

        Assert.That(project.ReleasesAccessLevel, Is.EqualTo(ReleasesAccessLevel.Enabled));
        Assert.That(release.TagName, Is.EqualTo("0.7"));
        Assert.That(release.Name, Is.EqualTo("0.7"));
        Assert.That(release.Description, Is.EqualTo("test"));
        Assert.That(release.Links.Self, Is.EqualTo($"{project.WebUrl}/-/releases/0.7"));
        Assert.That(release.Links.EditUrl, Is.EqualTo($"{project.WebUrl}/-/releases/0.7/edit"));

        Assert.That(releaseClient.GetAsync().FirstOrDefault(x => string.Equals(x.Name, "0.7", StringComparison.Ordinal)), Is.Not.Null);

        release = releaseClient[tag.Name];
        Assert.That(release.TagName, Is.EqualTo("0.7"));
        Assert.That(release.Name, Is.EqualTo("0.7"));
        Assert.That(release.Description, Is.EqualTo("test"));

        release = releaseClient.Update(new ReleaseUpdate
        {
            TagName = "0.7",
            Description = "test updated",
        });
        Assert.That(release.TagName, Is.EqualTo("0.7"));
        Assert.That(release.Name, Is.EqualTo("0.7"));
        Assert.That(release.Description, Is.EqualTo("test updated"));

        Assert.That(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "0.7", StringComparison.Ordinal)), Is.Not.Null);
        tagsClient.Delete("0.7");
        Assert.That(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "0.7", StringComparison.Ordinal)), Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_release_links()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var tagsClient = context.Client.GetRepository(project.Id).Tags;
        var releaseClient = context.Client.GetReleases(project.Id);

        var tag = tagsClient.Create(new TagCreate
        {
            Name = "0.7",
            Ref = project.DefaultBranch,
        });

        var release = releaseClient.Create(new ReleaseCreate
        {
            TagName = tag.Name,
            Description = "test",
        });
        Assert.That(release.TagName, Is.EqualTo("0.7"));
        Assert.That(release.Name, Is.EqualTo("0.7"));
        Assert.That(release.Description, Is.EqualTo("test"));

        var linksClient = releaseClient.ReleaseLinks(tag.Name);

        var link = linksClient.Create(new ReleaseLinkCreate
        {
            Name = "test link",
            Filepath = "/bin/test",
            Url = "https://www.example.com",
        });
        Assert.That(link.Name, Is.EqualTo("test link"));
        Assert.That(link.Url, Is.EqualTo("https://www.example.com"));

        if (context.IsGitLabMajorVersion(15))
        {
            Assert.That(link.External, Is.True);
        }

        link = linksClient[link.Id.Value];
        Assert.That(link.Name, Is.EqualTo("test link"));
        Assert.That(link.Url, Is.EqualTo("https://www.example.com"));

        if (context.IsGitLabMajorVersion(15))
        {
            Assert.That(link.External, Is.True);
        }

        linksClient.Delete(link.Id.Value);
        Assert.That(linksClient.All.FirstOrDefault(x => string.Equals(x.Name, "test link", StringComparison.Ordinal)), Is.Null);

        tagsClient.Delete("0.7");
        Assert.That(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "0.7", StringComparison.Ordinal)), Is.Null);
    }
}
