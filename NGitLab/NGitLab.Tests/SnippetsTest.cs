using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using NGitLab.Impl;
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
                FileName = "testFileName.cs",
                Visibility = VisibilityLevel.Public,
            };

            var testProjectId = Initialize.UnitTestProject.Id;

            var newSnippet2 = new SnippetProjectCreate
            {
                Title = "testSnipInProject",
                Code = "var test = 43;",
                FileName = "testFileName1.cs",
                ProjectId = testProjectId,
                Visibility = VisibilityLevel.Public
            };

            //act - assert
            SnippetClient.Create(newSnippet1);
            Assert.That(SnippetClient.User.Select(x => x.Title), Contains.Item("testSnip"));
            Assert.That(SnippetClient.All.Select(x => x.Title), Contains.Item("testSnip"));

            SnippetClient.Create(newSnippet2);
            Assert.That(SnippetClient.All.Select(x => x.Title), Contains.Item("testSnipInProject"));

            var returnedUserSnippet = SnippetClient.All.First(s => s.Title == "testSnip");
            var returnedProjectSnippet = SnippetClient.All.First(s => s.Title == "testSnipInProject");

            Assert.That(SnippetClient.Get(newSnippet2.ProjectId, returnedProjectSnippet.Id), Is.Not.Null);

            SnippetClient.Delete(returnedUserSnippet.Id);
            SnippetClient.Delete(newSnippet2.ProjectId, returnedProjectSnippet.Id);
        }
    }
}
