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
                    new SnippetCreateFile() { Action = SnippetCreateFile.ActionType.Create, FilePath = "Path1.txt", Content = "Content1" },
                    new SnippetCreateFile {  Action = SnippetCreateFile.ActionType.Create, FilePath = "Path2.txt", Content = "Content2" },
                },
            };

            // act - assert
            snippetClient.Create(newSnippet1);
            var snippet = snippetClient.All.First();

            Assert.IsNotNull(snippet.Files);
            Assert.IsNotEmpty(snippet.Files);
            Assert.IsTrue(snippet.Files.Length == 2);

            Assert.IsNotNull(snippet.Files[0]);
            Assert.IsTrue(string.Equals(snippet.Files[0].Path, "Path1.txt", StringComparison.Ordinal));
        }

        [Test]
        public void  Test_snippet_deserialize()
        {
            var body = @"{ ""id"": 6, ""title"": ""TITLE"", ""description"": ""DESCRIPTION"", ""visibility"": ""internal"", ""updated_at"": ""2022-06-16T12:50:47.646Z"", ""created_at"": ""2022-06-15T12:05:51.654Z"", ""project_id"": 1, ""web_url"": ""https://gitlab-xxxxx/-/snippets/2694"", ""raw_url"": ""https://gitlab-xxxxx/-/snippets/2694/raw"", ""ssh_url_to_repo"": ""git@gitlab-xxxxx/snippets/2694.git"", ""http_url_to_repo"": ""https://gitlab-xxxxx/snippets/2694.git"", ""author"": { ""id"": 1, ""name"": ""xxxx"", ""username"": ""dranc"", ""state"": ""active"", ""avatar_url"": ""https://gitlab-xxxxx/uploads/-/system/user/avatar/1/avatar.png"", ""web_url"": ""https://gitlab-xxxxx/dranc"" }, ""file_name"": ""ResourcesHelper.cs"", ""files"": [ { ""path"": ""ResourcesHelper.cs"", ""raw_url"": ""https:/gitlab-xxxxxx/-/snippets/1/raw/main/file1.cs"" }, { ""path"": ""SecondFile.txt"", ""raw_url"": ""https:/gitlab-xxxxx/-/snippets/1/raw/main/file2.txt"" } ] }";

            var snippet = Serializer.Deserialize<Snippet>(body);

            Assert.IsNotNull(snippet.Files);
            Assert.IsNotEmpty(snippet.Files);
            Assert.IsTrue(snippet.Files.Length == 2);

            Assert.IsNotNull(snippet.Files[0]);
            Assert.IsTrue(string.Equals(snippet.Files[0].Path, "ResourcesHelper.cs", StringComparison.Ordinal));
        }
    }
}
