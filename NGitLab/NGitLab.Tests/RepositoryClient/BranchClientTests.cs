using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class BranchClientTests {
        readonly IBranchClient branches;

        public BranchClientTests() {
            branches = _RepositoryClientTests.RepositoryClient.Branches;
        }

        [Test]
        [Category("Server_Required")]
        public void GetAll() {
            branches.All().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetByName() {
            var branch = branches.Get("master");
            Assert.IsNotNull(branch);
            Assert.IsNotNull(branch.Name);
        }

        [Test]
        [Category("Server_Required")]
        public void DeleteByName() {
            branches.Create(new BranchCreate {
                Name = "merge-me-to-master",
                Ref = "master"
            });
            var result = branches.Delete("merge-me-to-master");
            result.Succeed.ShouldBeTrue();
        }
    }
}