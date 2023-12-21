using System.Net;
using System.Threading.Tasks;
using NGitLab.Mock.Config;
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
                .WithCommit("Changes with tag", tags: new[] { "1.0.0" }))
            .BuildServer();

        var client = server.CreateClient();
        var tagClient = client.GetRepository(1).Tags;

        // Act/Assert
        var tag = await tagClient.GetByNameAsync("1.0.0");
        Assert.That(tag.Name, Is.EqualTo("1.0.0"));

        var ex = Assert.ThrowsAsync<GitLabException>(() => tagClient.GetByNameAsync("1.0.1"));
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
