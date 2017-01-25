using System;
using System.Linq;
using System.Threading;
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
            var fileUpsert = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = "test",
                Encoding = "base64",
                Path = "test.txt"
            };
            Files.Create(fileUpsert);

            var file = Files.Get("test.txt", "master");
            Assert.IsNotNull(file);
            Assert.AreEqual("test.txt", file.Name);
            Assert.AreEqual("test", file.DecodedContent);

            fileUpsert.RawContent = "test2";
            Files.Update(fileUpsert);

            file = Files.Get("test.txt", "master");
            Assert.IsNotNull(file);
            Assert.AreEqual("test2", file.DecodedContent);

            var fileDelete = new FileDelete()
            {
                Path = "test.txt",
                Branch = "master",
                CommitMessage = "Delete file"
            };
            Files.Delete(fileDelete);

            Assert.Throws<GitLabException>(() => Files.Get("testDelete.txt", "master"));
        }
    }
}