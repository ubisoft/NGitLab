using System.IO;
using System.Linq;
using System.Text;
using NGitLab.Mock.Clients;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class RepositoryMockTests
{
    [Test]
    public void Test_get_tree_when_subdirectory_does_not_exist_throws_not_found()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, configure: project => project
                .WithCommit("Initial commit"))
            .BuildServer();

        var client = server.CreateClient();
        var repository = client.GetRepository(1);

        // Act
        var handler = () => repository.GetTreeAsync(new RepositoryGetTreeOptions { Path = "non-existing-directory" });

        // Assert
        Assert.That(handler, Throws.TypeOf<GitLabNotFoundException>());
    }

    [Test]
    public void Test_get_tree_item_has_id_and_mode()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, configure: project => project
                .WithCommit("Initial commit", configure: commit => commit
                    .WithFile("file.txt", "content")))
            .BuildServer();

        var client = server.CreateClient();
        var repository = client.GetRepository(1);

        // Act
        var tree = repository.GetTreeAsync(new RepositoryGetTreeOptions());

        // Assert
        var item = tree.First(i => string.Equals(i.Path, "file.txt", System.StringComparison.Ordinal));
        Assert.Multiple(() =>
        {
            Assert.That(item.Id, Is.Not.EqualTo(default(Sha1)));
            Assert.That(item.Mode, Is.EqualTo("100644"));
        });
    }

    [Test]
    public void Test_get_raw_blob_content()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, configure: project => project
                .WithCommit("Initial commit", configure: commit => commit
                    .WithFile("file.txt", "content")))
            .BuildServer();

        var client = server.CreateClient();
        var repository = client.GetRepository(1);

        var tree = repository.GetTreeAsync(new RepositoryGetTreeOptions());
        var item = tree.First(i => string.Equals(i.Path, "file.txt", System.StringComparison.Ordinal));

        using var destination = new MemoryStream();

        // Act
        repository.GetRawBlob(item.Id.ToString(), source => source.CopyTo(destination));

        // Assert
        Assert.That(Encoding.UTF8.GetString(destination.ToArray()), Is.EqualTo("content"));
    }
}
