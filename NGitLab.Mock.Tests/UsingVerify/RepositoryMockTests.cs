using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Mock.Clients;
using NGitLab.Mock.Config;
using NUnit.Framework;
using static VerifyNUnit.Verifier;
using RepositoryGetTreeOptions = NGitLab.Models.RepositoryGetTreeOptions;
using Throws = NUnit.Framework.Throws;

namespace NGitLab.Mock.Tests.UsingVerify;

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
        Assert.That(handler, Throws.InstanceOf<GitLabNotFoundException>());
    }

    [Test]
    public async Task Test_get_tree_item_has_id_and_mode()
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
        await Verify(tree);
    }

    [Test]
    public async Task Test_get_tree_item_in_sub_folder()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, configure: project => project
                .WithCommit("Initial commit", configure: commit => commit
                    .WithFile("readme.md", "content")
                    .WithFile("subFolder/file.txt", "content")))
            .BuildServer();

        var client = server.CreateClient();
        var repository = client.GetRepository(1);

        // Act
        var tree = repository.GetTreeAsync(new RepositoryGetTreeOptions { Path = "subFolder" });

        // Assert
        await Verify(tree);
    }

    [Test]
    public async Task Test_get_tree_with_recurse()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, configure: project => project
                .WithCommit("Initial commit", configure: commit => commit
                    .WithFile("readme.md", "content")
                    .WithFile("subFolder/file.txt", "content")))
            .BuildServer();

        var client = server.CreateClient();
        var repository = client.GetRepository(1);

        // Act
        var tree = repository.GetTreeAsync(new RepositoryGetTreeOptions { Recursive = true });

        // Assert
        await Verify(tree);
    }

    [Test]
    public async Task Test_get_tree_not_in_main_branch()
    {
        // Arrange
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("test-project", id: 1, configure: project => project
                .WithCommit("Initial commit", configure: commit => commit
                    .WithFile("readme.md", "content"))
                .WithCommit("Second commit", sourceBranch: "feature", configure: commit => commit
                    .WithFile("subFolder/file.txt", "content")))
            .BuildServer();

        var client = server.CreateClient();
        var repository = client.GetRepository(1);

        // Act
        var tree = repository.GetTreeAsync(new RepositoryGetTreeOptions { Ref = "feature", Recursive = true });

        // Assert
        await Verify(tree);
    }

    [Test]
    public async Task Test_get_raw_blob_content()
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
        await Verify(destination);
    }
}
