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
            CollectionAssert.IsNotEmpty(commits);
            Assert.AreEqual(_commit.Message, commits[0].Message);
            Assert.AreEqual("add readme", commits.Last().Message);
        }

        [Test]
        public void GetCommitByBranchName()
        {
            CollectionAssert.IsNotEmpty(Initialize.Repository.GetCommits("master"));
            CollectionAssert.IsNotEmpty(Initialize.Repository.GetCommits("master", -1));

            var commits = Initialize.Repository.GetCommits("master", 1).ToArray();
            Assert.AreEqual(1, commits.Length);
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

        [Test]
        public void GetAllTreeInPath()
        {
            var tree = Initialize.Repository.GetTree("");
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public void GetAllTreeInNotGoodPath()
        {
            var tree = Initialize.Repository.GetTree("Fakepath");
            Assert.IsEmpty(tree);
        }
    }
}
