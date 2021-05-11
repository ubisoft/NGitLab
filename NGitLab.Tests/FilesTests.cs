using System;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class FilesTests
    {
        [Test]
        public async Task Test_add_update_delete_and_get_file()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var filesClient = context.Client.GetRepository(project.Id).Files;

            // Don't use txt extensions: https://gitlab.com/gitlab-org/gitlab-ce/issues/31790
            var fileName = "test.md";
            var fileUpsert = new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = "test",
                Encoding = "base64",
                Path = fileName,
            };
            filesClient.Create(fileUpsert);

            var file = filesClient.Get(fileName, project.DefaultBranch);
            Assert.IsNotNull(file);
            Assert.AreEqual(fileName, file.Name);
            Assert.AreEqual("test", file.DecodedContent);

            fileUpsert.RawContent = "test2";
            filesClient.Update(fileUpsert);

            file = filesClient.Get(fileName, project.DefaultBranch);
            Assert.IsNotNull(file);
            Assert.AreEqual("test2", file.DecodedContent);

            var fileDelete = new FileDelete()
            {
                Path = fileName,
                Branch = project.DefaultBranch,
                CommitMessage = "Delete file",
            };
            filesClient.Delete(fileDelete);

            Assert.Throws(Is.InstanceOf<GitLabException>(), () => filesClient.Get("testDelete.md", project.DefaultBranch));
        }

        [Test]
        public async Task Test_get_blame_of_latest_commit()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var filesClient = context.Client.GetRepository(project.Id).Files;

            var fileName = "blame_test_2.md";
            var content1 = "test";
            var fileUpsert1 = new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = $"{content1}{Environment.NewLine}",
                Encoding = "base64",
                Path = fileName,
            };
            filesClient.Create(fileUpsert1);

            var blameArray1 = filesClient.Blame(fileName, project.DefaultBranch);

            Assert.AreEqual(1, blameArray1.Length);
            Assert.IsNotNull(blameArray1);

            var firstBlameInfo = blameArray1[0];

            Assert.AreEqual(content1, string.Join(Environment.NewLine, firstBlameInfo.Lines));
            Assert.AreEqual(fileUpsert1.CommitMessage, firstBlameInfo.Commit.Message);
            Assert.NotNull(firstBlameInfo.Commit.CommittedDate);
            Assert.IsNotEmpty(firstBlameInfo.Commit.AuthorEmail);
            Assert.IsNotEmpty(firstBlameInfo.Commit.AuthorName);
            Assert.IsNotEmpty(firstBlameInfo.Commit.CommitterEmail);
            Assert.IsNotEmpty(firstBlameInfo.Commit.CommitterName);
            Assert.NotNull(firstBlameInfo.Commit.AuthoredDate);

            var content2 = "second line";
            var fileUpsert2 = new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "SecondCommit",
                RawContent = $"{content1}{Environment.NewLine}{content2}",
                Encoding = "base64",
                Path = fileName,
            };
            filesClient.Update(fileUpsert2);

            var blameArray2 = filesClient.Blame(fileName, project.DefaultBranch);

            Assert.AreEqual(2, blameArray2.Length);
            Assert.AreEqual(firstBlameInfo, blameArray2[0]);

            var secondBlameInfo = blameArray2[1];

            Assert.AreEqual(content2, string.Join(Environment.NewLine, secondBlameInfo.Lines));
            Assert.AreEqual(fileUpsert2.CommitMessage, secondBlameInfo.Commit.Message);
            Assert.NotNull(secondBlameInfo.Commit.CommittedDate);
            Assert.IsNotEmpty(secondBlameInfo.Commit.AuthorEmail);
            Assert.IsNotEmpty(secondBlameInfo.Commit.AuthorName);
            Assert.IsNotEmpty(secondBlameInfo.Commit.CommitterEmail);
            Assert.IsNotEmpty(secondBlameInfo.Commit.CommitterName);
            Assert.NotNull(secondBlameInfo.Commit.AuthoredDate);

            var fileDelete = new FileDelete()
            {
                Path = fileName,
                Branch = project.DefaultBranch,
                CommitMessage = "Delete file",
            };
            filesClient.Delete(fileDelete);
        }

        [Test]
        public async Task Test_get_blame_of_an_old_commit()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var filesClient = context.Client.GetRepository(project.Id).Files;

            var fileName = "blame_test_2.md";
            var content1 = $"test{Environment.NewLine}";
            var fileUpsert1 = new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = content1,
                Encoding = "base64",
                Path = fileName,
            };
            filesClient.Create(fileUpsert1);

            var initialBlame = filesClient.Blame(fileName, project.DefaultBranch);

            Assert.IsNotNull(initialBlame);
            Assert.AreEqual(1, initialBlame.Length);

            var initialBlameInfo = initialBlame[0];

            var content2 = "second line";
            var fileUpsert2 = new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = $"SecondCommit{Environment.NewLine}",
                RawContent = $"{content1}{content2}",
                Encoding = "base64",
                Path = fileName,
            };
            filesClient.Update(fileUpsert2);

            var blameById = filesClient.Blame(fileName, initialBlameInfo.Commit.Id.ToString());

            Assert.AreEqual(1, blameById.Length);
            Assert.AreEqual(initialBlameInfo, blameById[0]);

            var fileDelete = new FileDelete()
            {
                Path = fileName,
                Branch = project.DefaultBranch,
                CommitMessage = "Delete file",
            };
            filesClient.Delete(fileDelete);
        }

        [Test]
        public async Task Test_blame_comparison()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var filesClient = context.Client.GetRepository(project.Id).Files;

            var fileName = "blame_test_3.md";
            var content1 = $"test{Environment.NewLine}";
            var fileUpsert1 = new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = content1,
                Encoding = "base64",
                Path = fileName,
            };
            filesClient.Create(fileUpsert1);

            var realBlame = filesClient.Blame(fileName, project.DefaultBranch);

            Assert.IsNotNull(realBlame);
            Assert.AreEqual(1, realBlame.Length);

            var realBlameInfo = realBlame[0];

            var dummyBlameInfo = new Blame();

            Assert.AreNotEqual(dummyBlameInfo, realBlameInfo);
            Assert.AreNotEqual(realBlameInfo, null);
            Assert.AreNotEqual(null, realBlameInfo);
            Assert.AreEqual(realBlameInfo, realBlameInfo);
            Assert.AreEqual(dummyBlameInfo, dummyBlameInfo);

            var fileDelete = new FileDelete()
            {
                Path = fileName,
                Branch = project.DefaultBranch,
                CommitMessage = "Delete file",
            };
            filesClient.Delete(fileDelete);
        }
    }
}
