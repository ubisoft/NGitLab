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

            var randomProjectId = Initialize.GitLabClient.Projects.Accessible.First().Id;

            var newSnippet2 = new SnippetProjectCreate
            {
                Title = "testSnipInProject",
                Code = "var test = 43;",
                FileName = "testFileName1.cs",
                Id = randomProjectId,
                Visibility = VisibilityLevel.Public
            };

            //act - assert
            snippet.Create(newSnippet1);
            Assert.True(snippet.All.Any(s => s.Title == newSnippet1.Title));

            snippet.Create(newSnippet2);
            Assert.True(snippet.All.Any(s => s.Title == newSnippet2.Title));

            Assert.NotNull(snippet.Get(randomProjectId, snippet.All.First(s => s.Title == newSnippet2.Title).Id));

            var snippetUserId = snippet.All.OrderBy(s => s.Id).First().Id;

            snippet.Delete(snippetUserId);
            snippet.Delete(randomProjectId, snippet.All.First().Id);

            Assert.IsEmpty(snippet.All);
        }

        private ISnippetClient snippet => Initialize.GitLabClient.Snippets;
    }
}
