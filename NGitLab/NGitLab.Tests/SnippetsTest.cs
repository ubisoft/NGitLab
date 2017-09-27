using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class SnippetsTest
    {
        private ISnippetClient SnippetClient => Initialize.GitLabClient.Snippets;

        [Test]
        public void Test_snippet()
        {
            //arrange
            var newSnippet1 = new SnippetCreate
            {
                Title = "testSnip",
                Content = "var test = 42;",
                FileName = "testFileName.cs"
            };

            var testProjectId = Initialize.GitLabClient.Projects.Accessible.First(p => p.Name == Initialize.ProjectName).Id;

            var newSnippet2 = new SnippetProjectCreate
            {
                Title = "testSnipInProject",
                Code = "var test = 43;",
                FileName = "testFileName1.cs",
                Id = testProjectId,
                Visibility = VisibilityLevel.Public
            };

            //act - assert
            SnippetClient.Create(newSnippet1);
            Assert.True(SnippetClient.All.Any(s => s.Title == newSnippet1.Title));

            SnippetClient.Create(newSnippet2);
            Assert.True(SnippetClient.All.Any(s => s.Title == newSnippet2.Title));

            Assert.NotNull(SnippetClient.Get(testProjectId, SnippetClient.All.First(s => s.Title == newSnippet2.Title).Id));

            var snippetProjectId = SnippetClient.All.OrderBy(s => s.Id).Last().Id;

            SnippetClient.Delete(testProjectId, snippetProjectId);

            SnippetClient.All.ForEach(s => SnippetClient.Delete(s.Id));

            Assert.IsEmpty(SnippetClient.All);
        }
    }
}
