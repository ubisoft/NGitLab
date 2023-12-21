using System;
using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

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

        Assert.That(singleRelease, Is.Not.Null);
        Assert.That(singleRelease.TagName, Is.EqualTo("1.2.3"));
        Assert.That(singleRelease.Links.Self, Is.EqualTo($"{project.WebUrl}/-/releases/1.2.3"));
    }

    [Test]
    public void Test_release_page()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", configure: project => project
                .WithCommit("Changes with tag", tags: new[] { "1.2.3", "1.2.4" })
                .WithRelease("user1", "1.2.3", createdAt: DateTime.UtcNow.AddHours(-2), releasedAt: DateTime.UtcNow.AddHours(-2))
                .WithRelease("user1", "1.2.4", createdAt: DateTime.UtcNow.AddHours(-1), releasedAt: DateTime.UtcNow.AddHours(-1)))
            .BuildServer();

        var client = server.CreateClient("user1");
        var project = client.Projects.Visible.First();
        var releaseClient = client.GetReleases(project.Id);
        var firstRelease = releaseClient.GetAsync(new Models.ReleaseQuery
        {
            PerPage = 1,
            Page = 2,
        }).SingleOrDefault();

        Assert.That(firstRelease, Is.Not.Null);
        Assert.That(firstRelease.TagName, Is.EqualTo("1.2.3"));
    }

    [Test]
    public void Test_release_sort()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", configure: project => project
                .WithCommit("Changes with tag", tags: new[] { "1.2.3", "1.2.4" })
                .WithRelease("user1", "1.2.3", createdAt: DateTime.UtcNow.AddHours(-2), releasedAt: DateTime.UtcNow.AddHours(-2))
                .WithRelease("user1", "1.2.4", createdAt: DateTime.UtcNow.AddHours(-1), releasedAt: DateTime.UtcNow.AddHours(-1)))
            .BuildServer();

        var client = server.CreateClient("user1");
        var project = client.Projects.Visible.First();
        var releaseClient = client.GetReleases(project.Id);
        var firstRelease = releaseClient.GetAsync(new Models.ReleaseQuery
        {
            Sort = "asc",
        }).First();

        Assert.That(firstRelease, Is.Not.Null);
        Assert.That(firstRelease.TagName, Is.EqualTo("1.2.3"));
    }

    [Test]
    public void Test_release_orderBy()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", configure: project => project
                .WithCommit("Changes with tag", tags: new[] { "1.2.3", "1.2.4" })
                .WithRelease("user1", "1.2.3", createdAt: DateTime.UtcNow.AddHours(-1), releasedAt: DateTime.UtcNow.AddHours(-1))
                .WithRelease("user1", "1.2.4", createdAt: DateTime.UtcNow.AddHours(-2), releasedAt: DateTime.UtcNow.AddHours(-2)))
            .BuildServer();

        var client = server.CreateClient("user1");
        var project = client.Projects.Visible.First();
        var releaseClient = client.GetReleases(project.Id);
        var firstRelease = releaseClient.GetAsync(new Models.ReleaseQuery
        {
            OrderBy = "created_at",
        }).First();

        Assert.That(firstRelease, Is.Not.Null);
        Assert.That(firstRelease.TagName, Is.EqualTo("1.2.3"));
    }
}
