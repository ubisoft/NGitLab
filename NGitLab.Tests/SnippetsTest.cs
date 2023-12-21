using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class SnippetsTest
{
    [Test]
    [NGitLabRetry]
    public async Task Test_snippet_public()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var snippetClient = context.Client.Snippets;

        var guid = Guid.NewGuid().ToString("N");
        var snippetName = "testSnip" + guid;

        // arrange
        var newSnippet1 = new SnippetCreate
        {
            Title = snippetName,
            Visibility = VisibilityLevel.Public,
            Files = new[]
            {
                new SnippetCreateFile { FilePath = "Path1.txt", Content = "Content1" },
                new SnippetCreateFile { FilePath = "Path2.txt", Content = "Content2" },
            },
        };

        // act - assert
        snippetClient.Create(newSnippet1);
        Assert.That(snippetClient.User.Select(x => x.Title), Contains.Item(snippetName));
        Assert.That(snippetClient.All.Select(x => x.Title), Contains.Item(snippetName));

        var returnedUserSnippet = snippetClient.All.First(s => string.Equals(s.Title, snippetName, StringComparison.Ordinal));

        Assert.That(returnedUserSnippet.Files, Is.Not.Null);
        Assert.That(returnedUserSnippet.Files, Has.Length.EqualTo(2));

        Assert.That(returnedUserSnippet.Files[0], Is.Not.Null);
        Assert.That(string.Equals(returnedUserSnippet.Files[0].Path, "Path1.txt", StringComparison.Ordinal), Is.True);

        var updatedSnippet = new SnippetUpdate
        {
            SnippetId = returnedUserSnippet.Id,
            Title = snippetName,
            Visibility = VisibilityLevel.Public,
            Files = new[]
            {
                new SnippetUpdateFile { Action = SnippetUpdateFileAction.Delete, FilePath = returnedUserSnippet.Files[0].Path },
                new SnippetUpdateFile { Action = SnippetUpdateFileAction.Move, PreviousFile = returnedUserSnippet.Files[1].Path, FilePath = "rename.md" },
            },
        };

        snippetClient.Update(updatedSnippet);

        var returnedProjectSnippetAfterUpdate = snippetClient.User.First(s => string.Equals(s.Title, snippetName, StringComparison.Ordinal));

        Assert.That(returnedProjectSnippetAfterUpdate.Files, Has.Length.EqualTo(1));
        Assert.That(returnedProjectSnippetAfterUpdate.Files[0].Path, Is.EqualTo("rename.md"));

        snippetClient.Delete(returnedUserSnippet.Id);
    }

    [TestCase(VisibilityLevel.Private)]
    [TestCase(VisibilityLevel.Internal)]
    [TestCase(VisibilityLevel.Public)]
    [NGitLabRetry]
    public async Task Test_snippet_inProject(VisibilityLevel visibility)
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var snippetClient = context.Client.Snippets;

        var guid = Guid.NewGuid().ToString("N");
        var projectSnippetName = "testSnipInProject" + guid;

        // arrange
        var testProjectId = project.Id;

        var newSnippet = new SnippetProjectCreate
        {
            Title = projectSnippetName,
            ProjectId = testProjectId,
            Visibility = visibility,
            Files = new[]
            {
                new SnippetCreateFile { FilePath = "Path1.txt", Content = "Content1" },
                new SnippetCreateFile { FilePath = "Path2.txt", Content = "Content2" },
            },
        };

        // act - assert
        snippetClient.Create(newSnippet);
        Assert.That(snippetClient.User.Select(x => x.Title), Contains.Item(projectSnippetName));

        var returnedProjectSnippet = snippetClient.User.First(s => string.Equals(s.Title, projectSnippetName, StringComparison.Ordinal));

        Assert.That(snippetClient.Get(newSnippet.ProjectId, returnedProjectSnippet.Id), Is.Not.Null);

        Assert.That(returnedProjectSnippet.Files, Is.Not.Null);
        Assert.That(returnedProjectSnippet.Files, Has.Length.EqualTo(2));

        Assert.That(returnedProjectSnippet.Files[0], Is.Not.Null);
        Assert.That(string.Equals(returnedProjectSnippet.Files[0].Path, "Path1.txt", StringComparison.Ordinal), Is.True);

        var updatedSnippet = new SnippetProjectUpdate
        {
            SnippetId = returnedProjectSnippet.Id,
            Title = projectSnippetName,
            ProjectId = testProjectId,
            Visibility = visibility,
            Files = new[]
            {
                new SnippetUpdateFile { Action = SnippetUpdateFileAction.Delete, FilePath = returnedProjectSnippet.Files[0].Path },
                new SnippetUpdateFile { Action = SnippetUpdateFileAction.Move, PreviousFile = returnedProjectSnippet.Files[1].Path, FilePath = "rename.md" },
            },
        };

        snippetClient.Update(updatedSnippet);

        var returnedProjectSnippetAfterUpdate = snippetClient.User.First(s => string.Equals(s.Title, projectSnippetName, StringComparison.Ordinal));

        Assert.That(returnedProjectSnippetAfterUpdate.Files, Has.Length.EqualTo(1));
        Assert.That(returnedProjectSnippetAfterUpdate.Files[0].Path, Is.EqualTo("rename.md"));

        snippetClient.Delete(newSnippet.ProjectId, returnedProjectSnippet.Id);
    }
}
