using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

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

        Assert.That(result, Is.Not.Null);
        Assert.That(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", StringComparison.Ordinal)), Is.Not.Null);
        Assert.That(tagsClient.All.FirstOrDefault(x => string.Equals(x.Message, "Test message", StringComparison.Ordinal)), Is.Not.Null);

        tagsClient.Delete("v0.5");
        Assert.That(tagsClient.All.FirstOrDefault(x => string.Equals(x.Name, "v0.5", StringComparison.Ordinal)), Is.Null);
    }

    [NGitLabRetry]
    [Test]
    public async Task SearchTags()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var tagClient = context.Client.GetRepository(project.Id).Tags;

        tagClient.Create(new TagCreate
        {
            Name = "v0.5",
            Message = "First tag",
            Ref = project.DefaultBranch,
        });

        tagClient.Create(new TagCreate
        {
            Name = "v0.5.1",
            Message = "Second tag",
            Ref = project.DefaultBranch,
        });

        tagClient.Create(new TagCreate
        {
            Name = "v0.6",
            Message = "Third tag",
            Ref = project.DefaultBranch,
        });

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
        Assert.That(tagCreated, Is.Not.Null);

        // Act/Assert
        if (expectExistence)
        {
            var tagFetched = await tagClient.GetByNameAsync(tagNameSought);
            Assert.That(tagFetched, Is.Not.Null);
        }
        else
        {
            var ex = Assert.ThrowsAsync<GitLabException>(() => tagClient.GetByNameAsync(tagNameSought));
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [NGitLabRetry]
    [Test]
    public async Task EnumerateTags_FivePerPageAndWithStartPageSpecified()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var tagClient = context.Client.GetRepository(project.Id).Tags;
        var perPage = 5;

        for (var i = 0; i < 20; i++)
        {
            tagClient.Create(new TagCreate
            {
                Name = $"1.{i}.0",
                Ref = project.DefaultBranch,
            });
        }

        (int? StartPage, string[] ExpectedTags)[] testCases =
        [
            (null, ["1.19.0", "1.18.0", "1.17.0", "1.16.0", "1.15.0"]),
            (1, ["1.19.0", "1.18.0", "1.17.0", "1.16.0", "1.15.0"]),
            (2, ["1.14.0", "1.13.0", "1.12.0", "1.11.0", "1.10.0"]),
            (4, ["1.4.0", "1.3.0", "1.2.0", "1.1.0", "1.0.0"]),
            (5, []),
        ];

        foreach (var (startPage, expectedTags) in testCases)
        {
            // Act
            var tags = tagClient.GetAsync(new TagQuery { OrderBy = "version", PerPage = perPage, Page = startPage }).AsEnumerable().Take(perPage).ToArray();

            // Assert
            Assert.That(tags.Select(t => t.Name), Is.EqualTo(expectedTags));
        }
    }

    [NGitLabRetry]
    [Test]
    public async Task EnumerateTags_WithPreviousTagSpecified()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var tagClient = context.Client.GetRepository(project.Id).Tags;

        for (var i = 0; i < 10; i++)
        {
            tagClient.Create(new TagCreate
            {
                Name = $"1.{i}.0",
                Ref = project.DefaultBranch,
            });
        }

        (string PreviousTag, string[] ExpectedTags)[] testCases =
        [
            (null, ["1.9.0", "1.8.0", "1.7.0", "1.6.0", "1.5.0", "1.4.0", "1.3.0", "1.2.0", "1.1.0", "1.0.0"]),
            ("1.3.0", ["1.2.0", "1.1.0", "1.0.0"]),
        ];

        foreach (var (previousTag, expectedTags) in testCases)
        {
            // Act
            var tags = tagClient.GetAsync(new TagQuery { OrderBy = "version", PageToken = previousTag }).AsEnumerable().ToArray();

            // Assert
            Assert.That(tags.Select(t => t.Name), Is.EqualTo(expectedTags));
        }
    }
}
