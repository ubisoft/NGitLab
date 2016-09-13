using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class RepositoryClientTests
    {
        private readonly IRepositoryClient _repo;
        private IRepositoryClient RepositoryClient;

        [SetUp]
        public void Setup()
        {
            var project = Initialize.GitLabClient.Projects.Owned.First();
            RepositoryClient = Initialize.GitLabClient.GetRepository(project.Id);
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void GetAllCommits()
        {
            CollectionAssert.IsNotEmpty(_repo.Commits.ToArray());
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void GetCommitBySha1()
        {
            var sha1 = new Sha1("6104942438c14ec7bd21c6cd5bd995272b3faff6");
            Assert.AreEqual(sha1, _repo.GetCommit(sha1).Id);
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void GetCommitDiff()
        {
            CollectionAssert.IsNotEmpty(_repo.GetCommitDiff(_repo.Commits.First().Id).ToArray());
        }
    }
}