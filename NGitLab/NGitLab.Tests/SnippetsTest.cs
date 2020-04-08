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
        [Ignore("GitLab 12.9: staging GitLab breaks this. Revert this as soon as 12.9 is fixed.")]
        public void Test_snippet()
        {
            var guid = Guid.NewGuid().ToString("N");

            var snippetName = "testSnip" + guid;
            var projectSnippetName = "testSnipInProject" + guid;

            // arrange
            var newSnippet1 = new SnippetCreate
            {
                Title = snippetName,
                Content = "var test = 42;",
                FileName = "testFileName.cs",
                Visibility = VisibilityLevel.Public,
            };

            var testProjectId = Initialize.UnitTestProject.Id;

            var newSnippet2 = new SnippetProjectCreate
            {
                Title = projectSnippetName,
                Code = "var test = 43;",
                FileName = "testFileName1.cs",
                ProjectId = testProjectId,
                Visibility = VisibilityLevel.Public,
            };

            // act - assert
            SnippetClient.Create(newSnippet1);
            Assert.That(SnippetClient.User.Select(x => x.Title), Contains.Item(snippetName));
            Assert.That(SnippetClient.All.Select(x => x.Title), Contains.Item(snippetName));

            SnippetClient.Create(newSnippet2);
            Assert.That(SnippetClient.All.Select(x => x.Title), Contains.Item(projectSnippetName));

            var returnedUserSnippet = SnippetClient.All.First(s => string.Equals(s.Title, snippetName, StringComparison.Ordinal));
            var returnedProjectSnippet = SnippetClient.All.First(s => string.Equals(s.Title, projectSnippetName, StringComparison.Ordinal));

            Assert.That(SnippetClient.Get(newSnippet2.ProjectId, returnedProjectSnippet.Id), Is.Not.Null);

            SnippetClient.Delete(returnedUserSnippet.Id);
            SnippetClient.Delete(newSnippet2.ProjectId, returnedProjectSnippet.Id);
        }
    }
}
