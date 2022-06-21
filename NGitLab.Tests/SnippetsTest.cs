using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Impl.Json;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
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

            Assert.IsNotNull(returnedUserSnippet.Files);
            Assert.AreEqual(2, returnedUserSnippet.Files.Length);

            Assert.IsNotNull(returnedUserSnippet.Files[0]);
            Assert.IsTrue(string.Equals(returnedUserSnippet.Files[0].Path, "Path1.txt", StringComparison.Ordinal));

            var updatedSnippet = new SnippetUpdate
            {
                SnippedId = returnedUserSnippet.Id,
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

            Assert.AreEqual(1, returnedProjectSnippetAfterUpdate.Files.Length);
            Assert.AreEqual("rename.md", returnedProjectSnippetAfterUpdate.Files[0].Path);

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

            Assert.IsNotNull(returnedProjectSnippet.Files);
            Assert.AreEqual(2, returnedProjectSnippet.Files.Length);

            Assert.IsNotNull(returnedProjectSnippet.Files[0]);
            Assert.IsTrue(string.Equals(returnedProjectSnippet.Files[0].Path, "Path1.txt", StringComparison.Ordinal));

            var updatedSnippet = new SnippetProjectUpdate
            {
                SnippedId = returnedProjectSnippet.Id,
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

            Assert.AreEqual(1, returnedProjectSnippetAfterUpdate.Files.Length);
            Assert.AreEqual("rename.md", returnedProjectSnippetAfterUpdate.Files[0].Path);

            snippetClient.Delete(newSnippet.ProjectId, returnedProjectSnippet.Id);
        }
    }
}
