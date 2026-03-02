using System;
using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class ProjectSearchTests
{
    [Test]
    public void ShouldSearchInProjectByFilePath()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1)
            .BuildServer();

        var user = server.Users.Single();
        var project = server.AllProjects.Single();

        project.Repository.Commit(user, "Initial commit",
        [
            File.CreateFromText($"subfolder/README.md", "# Title"),
        ]);

        var client = server.CreateClient();

        var searchClient = client.GetProjectSearchClient(project.Id);
        var result = searchClient.GetBlobsAsync(new SearchQuery { Search = "path:subfolder/README.md" });

        var blob = result.Single();
        Assert.That(blob.FileName, Is.EqualTo("README.md"));
        Assert.That(blob.Path, Is.EqualTo("subfolder/README.md"));
    }

    [Test]
    public void ShouldSearchInProjectByFilename()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1)
            .BuildServer();

        var user = server.Users.Single();
        var project = server.AllProjects.Single();

        project.Repository.Commit(user, "Initial commit",
        [
            File.CreateFromText($"subfolder/README.md", "# Title"),
        ]);

        var client = server.CreateClient();

        var searchClient = client.GetProjectSearchClient(project.Id);
        var result = searchClient.GetBlobsAsync(new SearchQuery { Search = "filename:README.md" });

        var blob = result.Single();
        Assert.That(blob.FileName, Is.EqualTo("README.md"));
        Assert.That(blob.Path, Is.EqualTo("subfolder/README.md"));
    }
}
