using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class BranchClientTests
    {
        private readonly IBranchClient _branches;
        private IRepositoryClient RepositoryClient;

        [SetUp]
        public void Setup()
        {
            var project = Initialize.GitLabClient.Projects.Owned.First();
            RepositoryClient = Initialize.GitLabClient.GetRepository(project.Id);
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void GetAll()
        {
            CollectionAssert.IsNotEmpty(_branches.All.ToArray());
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void GetByName()
        {
            var branch = _branches["master"];
            Assert.IsNotNull(branch);
            Assert.IsNotNull(branch.Name);
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void DeleteByName()
        {
            var result = _branches.Delete("merge-me-to-master");
            Assert.That(result.Succeed);
        }
    }
}