using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class RepositoryClientTests {
        readonly IRepositoryClient _repo;

        public RepositoryClientTests() {
            _repo = _RepositoryClientTests.RepositoryClient;
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllCommits() {
            _repo.Commits.ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetCommitBySha1() {
            var commits = _repo.Commits.First().Id;
            _repo.GetCommit(commits).Id.ShouldNotBeNull();
        }

        [Test]
        [Category("Server_Required")]
        public void GetCommitDiff() {
            _repo.GetCommitDiff(_repo.Commits.First().Id).ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void CreateTag() {
            var tagCreate = new TagCreate();
            tagCreate.Ref = "master";
            tagCreate.TagName = "my_test_tag";

            Assert.IsNotNull(_repo.CreateTag(tagCreate));
        }
    }
}