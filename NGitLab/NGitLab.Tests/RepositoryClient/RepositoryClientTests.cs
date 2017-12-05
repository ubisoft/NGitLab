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
            var commits = Initialize.Repository.Commits.ToArray();
            Assert.AreEqual(2, commits.Length);
            Assert.AreEqual(_commit.Message, commits[0].Message);
            Assert.AreEqual("add readme", commits[1].Message);
        }

        [Test]
        public void GetCommitByBranchName()
        {
            Assert.AreEqual(2, Initialize.Repository.GetCommits("master").Count());
            Assert.AreEqual(2, Initialize.Repository.GetCommits("master", -1).Count());

            var commits = Initialize.Repository.GetCommits("master", 1).ToArray();
            Assert.AreEqual(1, commits.Count());
            Assert.AreEqual(_commit.Message, commits[0].Message);
        }

        [Test]
        public void GetCommitBySha1()
        {
            var sha1 = _commit.Id;
            var commit = Initialize.Repository.GetCommit(sha1);
            Assert.AreEqual(sha1, commit.Id);
            Assert.AreEqual(_commit.Message, commit.Message);
        }

        [Test]
        public void GetCommitDiff()
        {
            CollectionAssert.IsNotEmpty(Initialize.Repository.GetCommitDiff(Initialize.Repository.Commits.First().Id).ToArray());
        }
    }
}