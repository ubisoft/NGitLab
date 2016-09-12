using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class RepositoryClientTests
    {
        private readonly IRepositoryClient _repo;

        public RepositoryClientTests()
        {
            _repo = _RepositoryClientTests.RepositoryClient;
        }

        [Test]
        public void GetAllCommits()
        {
            CollectionAssert.IsNotEmpty(_repo.Commits.ToArray());
        }

        [Test]
        public void GetCommitBySha1()
        {
            var sha1 = new Sha1("6104942438c14ec7bd21c6cd5bd995272b3faff6");
            Assert.AreEqual(sha1, _repo.GetCommit(sha1).Id);
        }

        [Test]
        public void GetCommitDiff()
        {
            CollectionAssert.IsNotEmpty(_repo.GetCommitDiff(_repo.Commits.First().Id).ToArray());
        }
    }
}