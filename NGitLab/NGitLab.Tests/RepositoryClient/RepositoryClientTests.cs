using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class RepositoryClientTests
    {
        private Commit _commit;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var upsert = new FileUpsert
            {
                RawContent = "test",
                CommitMessage = "Commit for RepositoryClientTests",
                Path = "RepositoryClientTests.txt",
                Branch = "master",
            };

            Initialize.Repository.Files.Create(upsert);
            _commit = Initialize.Repository.Commits.First();

            Assert.AreEqual(upsert.CommitMessage, _commit.Message);
        }

        [Test]
        public void GetAllCommits()
        {
            CollectionAssert.IsNotEmpty(Initialize.Repository.Commits.ToArray());
        }

        [Test]
        public void GetCommitBySha1()
        {
            var sha1 = _commit.Id;
            Assert.AreEqual(sha1, Initialize.Repository.GetCommit(sha1).Id);
        }

        [Test]
        public void GetCommitDiff()
        {
            CollectionAssert.IsNotEmpty(Initialize.Repository.GetCommitDiff(Initialize.Repository.Commits.First().Id).ToArray());
        }
    }
}