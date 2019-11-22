using System;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class FilesTests
    {
        private IFilesClient Files
        {
            get
            {
                Assert.IsNotNull(Initialize.UnitTestProject);
                return Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Files;
            }
        }

        [Test]
        public void Test_add_update_delete_and_get_file()
        {
            // Don't use txt extensions: https://gitlab.com/gitlab-org/gitlab-ce/issues/31790
            var fileName = "test.md";
            var fileUpsert = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = "test",
                Encoding = "base64",
                Path = fileName
            };
            Files.Create(fileUpsert);

            var file = Files.Get(fileName, "master");
            Assert.IsNotNull(file);
            Assert.AreEqual(fileName, file.Name);
            Assert.AreEqual("test", file.DecodedContent);

            fileUpsert.RawContent = "test2";
            Files.Update(fileUpsert);

            file = Files.Get(fileName, "master");
            Assert.IsNotNull(file);
            Assert.AreEqual("test2", file.DecodedContent);

            var fileDelete = new FileDelete()
            {
                Path = fileName,
                Branch = "master",
                CommitMessage = "Delete file"
            };
            Files.Delete(fileDelete);

            Assert.Throws(Is.InstanceOf<GitLabException>(), () => Files.Get("testDelete.md", "master"));
        }

        [Test]
        public void Test_get_blame_of_latest_commit()
        {
            var fileName = "blame_test_2.md";
            var content1 = "test";
            var fileUpsert1 = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = $"{content1}{Environment.NewLine}",
                Encoding = "base64",
                Path = fileName
            };
            Files.Create(fileUpsert1);

            var blameArray1 = Files.Blame(fileName, "master");

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
                Branch = "master",
                CommitMessage = "SecondCommit",
                RawContent = $"{content1}{Environment.NewLine}{content2}",
                Encoding = "base64",
                Path = fileName
            };
            Files.Update(fileUpsert2);

            var blameArray2 = Files.Blame(fileName, "master");

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
                Branch = "master",
                CommitMessage = "Delete file"
            };
            Files.Delete(fileDelete);
        }

        [Test]
        public void Test_get_blame_of_an_old_commit()
        {
            var fileName = "blame_test_2.md";
            var content1 = $"test{Environment.NewLine}";
            var fileUpsert1 = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = content1,
                Encoding = "base64",
                Path = fileName
            };
            Files.Create(fileUpsert1);

            var initialBlame = Files.Blame(fileName, "master");

            Assert.IsNotNull(initialBlame);
            Assert.AreEqual(1, initialBlame.Length);
            
            var initialBlameInfo = initialBlame[0];

            var content2 = "second line";
            var fileUpsert2 = new FileUpsert
            {
                Branch = "master",
                CommitMessage = $"SecondCommit{Environment.NewLine}",
                RawContent = $"{content1}{content2}",
                Encoding = "base64",
                Path = fileName
            };
            Files.Update(fileUpsert2);

            var blameById = Files.Blame(fileName, initialBlameInfo.Commit.Id.ToString());

            Assert.AreEqual(1, blameById.Length);
            Assert.AreEqual(initialBlameInfo, blameById[0]);

            var fileDelete = new FileDelete()
            {
                Path = fileName,
                Branch = "master",
                CommitMessage = "Delete file"
            };
            Files.Delete(fileDelete);
        }

        [Test]
        public void Test_blame_comparison()
        {
            var fileName = "blame_test_3.md";
            var content1 = $"test{Environment.NewLine}";
            var fileUpsert1 = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = content1,
                Encoding = "base64",
                Path = fileName
            };
            Files.Create(fileUpsert1);

            var realBlame = Files.Blame(fileName, "master");

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
                Branch = "master",
                CommitMessage = "Delete file"
            };
            Files.Delete(fileDelete);
        }
    }
}