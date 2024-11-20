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
    [TestCase("^v0.5", 1)]
    [TestCase("^v0", 2)]
    [TestCase("^v1", 0)]
    [TestCase("v1", 0)]
    [TestCase("0.5$", 1)]
    [TestCase("0\\.", 0)]
    [TestCase(".5$", 1)]
    [TestCase("\\.5$", 0)]
    [TestCase(".[0-9]$", 0)]
    public async Task SearchTags(string search, int expectedCount)
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var tagClient = context.Client.GetRepository(project.Id).Tags;

        tagClient.Create(new TagCreate
        {
            Name = "v0.5",
            Message = "Test message",
            Ref = project.DefaultBranch,
        });

        tagClient.Create(new TagCreate
        {
            Name = "v0.6",
            Message = "Test second message",
            Ref = project.DefaultBranch,
        });

        var tagFetched = tagClient.GetAsync(new TagQuery { Search = search });
        Assert.That(tagFetched.Count(), Is.EqualTo(expectedCount));
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
}
