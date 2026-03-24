using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class TagTests
{
    [Test]
    public async Task GetTagAsync()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithCommit("Initial commit")
                .WithCommit("Changes with tag", tags: ["1.0.0"]))
            .BuildServer();

        var client = server.CreateClient();
        var tagClient = client.GetRepository(1).Tags;

        // Act/Assert
        var tag = await tagClient.GetByNameAsync("1.0.0");
        Assert.That(tag.Name, Is.EqualTo("1.0.0"));

        var ex = Assert.ThrowsAsync<GitLabException>(() => tagClient.GetByNameAsync("1.0.1"));
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Theory]
    public void GetTagsAsync_CanSortByName([Values] bool useDefault)
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithCommit("Initial Commit", tags: ["0.0.1"])
                .WithCommit("Second Tag", tags: ["0.0.2"])
                .WithCommit("Second Tag", tags: ["not-semver"])
                .WithCommit("Other Tag", tags: ["0.0.10"]))
            .BuildServer();

        var client = server.CreateClient();
        var tagClient = client.GetRepository(1).Tags;

        var query = new TagQuery
        {
            OrderBy = useDefault ? null : "name",
            Sort = "asc",
        };

        // Act
        var tags = tagClient.GetAsync(query);

        // Assert
        Assert.That(tags.AsEnumerable().Select(t => t.Name), Is.EqualTo(["0.0.1", "0.0.10", "0.0.2", "not-semver"]));
    }

    [Test]
    public void GetTagsAsync_CanSortByVersion()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithCommit("Initial Commit", tags: ["0.0.1"])
                .WithCommit("Second Tag", tags: ["0.0.2"])
                .WithCommit("Second Tag", tags: ["not-semver"])
                .WithCommit("Other Tag", tags: ["0.0.10"]))
            .BuildServer();

        var client = server.CreateClient();
        var tagClient = client.GetRepository(1).Tags;

        var query = new TagQuery
        {
            OrderBy = "version",
            Sort = "asc",
        };

        // Act
        var tags = tagClient.GetAsync(query);

        // Assert
        Assert.That(tags.AsEnumerable().Select(t => t.Name), Is.EqualTo(["not-semver", "0.0.1", "0.0.2", "0.0.10"]));
    }

    [Test]
    public async Task SearchTags()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, addDefaultUserAsMaintainer: true, configure: project => project
                .WithCommit("First Tag", tags: ["v0.5"])
                .WithCommit("Second Tag", tags: ["v0.5.1"])
                .WithCommit("Third Tag", tags: ["v0.6"]))
            .BuildServer();

        var client = server.CreateClient();
        var tagClient = client.GetRepository(1).Tags;

        (string, int)[] testCases =
        [
            // You can use "^term" and "term$" to find tags that begin and end with "term". No other regular expressions are supported.
            // The search expression is case-insensitive.
            // https://docs.gitlab.com/api/tags/#list-all-project-repository-tags
            ("^v0.5", 2),
            ("^v0", 3),
            ("^v", 3),
            ("^V", 3),
            ("^v1", 0),
            ("0.5", 2),
            ("0", 3),
            ("6", 1),
            ("V", 3),
            ("0.5$", 1),
            (".5$", 1),
            ("\\.5$", 0),
            (".[0-9]$", 0),
            ("0\\.", 0),
            ("^v0.5$", 1),
        ];

        foreach (var (searchExpression, expectedCount) in testCases)
        {
            // Act
            var tags = tagClient.GetAsync(new TagQuery { Search = searchExpression });

            // Assert
            Assert.That(tags.Count(), Is.EqualTo(expectedCount), $"Expected search expression '{searchExpression}' to return {expectedCount} results.");
        }
    }
}
