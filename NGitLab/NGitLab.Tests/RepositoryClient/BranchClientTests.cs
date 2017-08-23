using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class BranchClientTests {
        readonly IBranchClient _branches;

        public BranchClientTests() {
            _branches = _RepositoryClientTests.RepositoryClient.Branches;
        }

        [Test]
        [Category("Server_Required")]
        public void GetAll() {
            _branches.All().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetByName() {
            var branch = _branches.Get("master");
            Assert.IsNotNull(branch);
            Assert.IsNotNull(branch.Name);
        }

        [Test]
        [Category("Server_Required")]
        public void DeleteByName() {
            _branches.Create(new BranchCreate {
                Name = "merge-me-to-master",
                Ref = "master"
            });
            var result = _branches.Delete("merge-me-to-master");
            result.Succeed.ShouldBeTrue();
        }
    }
}