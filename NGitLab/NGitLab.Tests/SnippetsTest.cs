using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class SnippetsTest
    {
        private static ISnippetClient SnippetClient => Initialize.GitLabClient.Snippets;

        [Test]
        public void Test_snippet_public()
        {
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
            SnippetClient.Create(newSnippet1);
            Assert.That(SnippetClient.User.Select(x => x.Title), Contains.Item(snippetName));
            Assert.That(SnippetClient.All.Select(x => x.Title), Contains.Item(snippetName));

            var returnedUserSnippet = SnippetClient.All.First(s => string.Equals(s.Title, snippetName, StringComparison.Ordinal));
            SnippetClient.Delete(returnedUserSnippet.Id);
        }

        [TestCase(VisibilityLevel.Private)]
        [TestCase(VisibilityLevel.Internal)]
        [TestCase(VisibilityLevel.Public)]
        public void Test_snippet_inProject(VisibilityLevel visibility)
        {
            var guid = Guid.NewGuid().ToString("N");

            var projectSnippetName = "testSnipInProject" + guid;

            // arrange
            var testProjectId = Initialize.UnitTestProject.Id;

            var newSnippet = new SnippetProjectCreate
            {
                Title = projectSnippetName,
                Code = "var test = 43;",
                FileName = "testFileName1.cs",
                ProjectId = testProjectId,
                Visibility = visibility,
            };

            // act - assert
            SnippetClient.Create(newSnippet);
            Assert.That(SnippetClient.User.Select(x => x.Title), Contains.Item(projectSnippetName));

            var returnedProjectSnippet = SnippetClient.User.First(s => string.Equals(s.Title, projectSnippetName, StringComparison.Ordinal));

            Assert.That(SnippetClient.Get(newSnippet.ProjectId, returnedProjectSnippet.Id), Is.Not.Null);

            SnippetClient.Delete(newSnippet.ProjectId, returnedProjectSnippet.Id);
        }
    }
}
