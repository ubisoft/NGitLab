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
                Content = "var test = 42;",
                FileName = "testFileName.cs",
                Visibility = VisibilityLevel.Public,
            };

            // act - assert
            snippetClient.Create(newSnippet1);
            Assert.That(snippetClient.User.Select(x => x.Title), Contains.Item(snippetName));
            Assert.That(snippetClient.All.Select(x => x.Title), Contains.Item(snippetName));

            var returnedUserSnippet = snippetClient.All.First(s => string.Equals(s.Title, snippetName, StringComparison.Ordinal));
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
                Code = "var test = 43;",
                FileName = "testFileName1.cs",
                ProjectId = testProjectId,
                Visibility = visibility,
            };

            // act - assert
            snippetClient.Create(newSnippet);
            Assert.That(snippetClient.User.Select(x => x.Title), Contains.Item(projectSnippetName));

            var returnedProjectSnippet = snippetClient.User.First(s => string.Equals(s.Title, projectSnippetName, StringComparison.Ordinal));

            Assert.That(snippetClient.Get(newSnippet.ProjectId, returnedProjectSnippet.Id), Is.Not.Null);

            snippetClient.Delete(newSnippet.ProjectId, returnedProjectSnippet.Id);
        }

        [TestCase(VisibilityLevel.Private)]
        [TestCase(VisibilityLevel.Internal)]
        [TestCase(VisibilityLevel.Public)]
        [NGitLabRetry]
        public async Task Test_snippet_files(VisibilityLevel visibility)
        {
            using var context = await GitLabTestContext.CreateAsync();
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
                    new SnippetCreateFile() { Action = SnippetCreateFile.ActionType.Create, FilePath = "Path1.txt", Content = "Content1" },
                    new SnippetCreateFile {  Action = SnippetCreateFile.ActionType.Create, FilePath = "Path2.txt", Content = "Content2" },
                },
            };

            // act - assert
            snippetClient.Create(newSnippet1);
            var returnedProjectSnippet = snippetClient.User.First(s => string.Equals(s.Title, snippetName, StringComparison.Ordinal));

            Assert.IsNotNull(returnedProjectSnippet.Files);
            Assert.AreEqual(2, returnedProjectSnippet.Files.Length);

            Assert.IsNotNull(returnedProjectSnippet.Files[0]);
            Assert.IsTrue(string.Equals(returnedProjectSnippet.Files[0].Path, "Path1.txt", StringComparison.Ordinal));
        }
    }
}
