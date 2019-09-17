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
            var fileUpsert = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "Add SonarQube badges to README.md",
                RawContent = "test",
                Encoding = "base64",
                Path = "test.md"
            };
            Files.Create(fileUpsert);

            var file = Files.Get("test.md", "master");
            Assert.IsNotNull(file);
            Assert.AreEqual("test.md", file.Name);
            Assert.AreEqual("test", file.DecodedContent);

            fileUpsert.RawContent = "test2";
            Files.Update(fileUpsert);

            file = Files.Get("test.md", "master");
            Assert.IsNotNull(file);
            Assert.AreEqual("test2", file.DecodedContent);

            var fileDelete = new FileDelete()
            {
                Path = "test.md",
                Branch = "master",
                CommitMessage = "Delete file"
            };
            Files.Delete(fileDelete);

            Assert.Throws(Is.InstanceOf<GitLabException>(), () => Files.Get("testDelete.md", "master"));
        }
    }
}