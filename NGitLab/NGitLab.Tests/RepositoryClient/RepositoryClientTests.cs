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
        public void GetCommitBySha1Range()
        {
            var allCommits = Initialize.Repository.Commits.Reverse().ToArray();
            var commitRequest = new GetCommitsRequest
            {
                RefName = $"{allCommits[1].Id}..{allCommits[3].Id}",
                FirstParent = true,
            };

            var commits = Initialize.Repository.GetCommits(commitRequest).Reverse().ToArray();
            Assert.AreEqual(allCommits[2].Id, commits[0].Id);
            Assert.AreEqual(allCommits[3].Id, commits[1].Id);
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
        public void GetAllTreeInPathRecursively()
        {
            var tree = Initialize.Repository.GetTree("", null, true);
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public void GetAllTreeInPathOnRef()
        {
            var tree = Initialize.Repository.GetTree("", "master", false);
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public void GetAllTreeInNotGoodPath()
        {
            var tree = Initialize.Repository.GetTree("Fakepath");
            Assert.IsEmpty(tree);
        }

        [TestCase(CommitRefType.All)]
        [TestCase(CommitRefType.Branch)]
        [TestCase(CommitRefType.Tag)]
        public void GetCommitRefs(CommitRefType type)
        {
            var commitRefs = Initialize.Repository.GetCommitRefs(Initialize.Repository.Commits.First().Id, type).ToArray();

            if (type == CommitRefType.Tag)
            {
                CollectionAssert.IsEmpty(commitRefs);
            }
            else
            {
                CollectionAssert.IsNotEmpty(commitRefs);
            }
        }
    }
}
