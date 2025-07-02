using System.IO;
using System.Threading.Tasks;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class FileTests
{
    [TestCase("README.md", true)]
    [TestCase("does-not-exist.md", false)]
    public async Task Test_get_raw_file_async(string fileToLookUp, bool expectSuccess)
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit",
        [
            File.CreateFromText("README.md", "This is the initial content"),
        ]);

        var client = server.CreateClient(user);
        var filesClient = client.GetRepository(project.Id).Files;

        string downloadedContent = null;

        // Act/Assert
        if (expectSuccess)
        {
            await filesClient.GetRawAsync(fileToLookUp, async stream =>
            {
                using var streamReader = new StreamReader(stream);
                downloadedContent = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            });
            Assert.That(downloadedContent, Is.EqualTo("This is the initial content"));
        }
        else
        {
            Assert.ThrowsAsync<GitLabException>(async () => await filesClient.GetRawAsync(fileToLookUp, _ => Task.CompletedTask).ConfigureAwait(false));
        }
    }
}
