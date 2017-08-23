using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class RepositoryClientTests {
        readonly IRepositoryClient repo;

        public RepositoryClientTests() {
            repo = _RepositoryClientTests.RepositoryClient;
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllCommits() {
            repo.Commits.ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetCommitBySha1() {
            var commits = repo.Commits.First().Id;
            repo.GetCommit(commits).Id.ShouldNotBeNull();
        }

        [Test]
        [Category("Server_Required")]
        public void GetCommitDiff() {
            repo.GetCommitDiff(repo.Commits.First().Id).ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void CreateTag() {
            var tagCreate = new TagCreate();
            tagCreate.Ref = "master";
            tagCreate.TagName = "my_test_tag";

            Assert.IsNotNull(repo.CreateTag(tagCreate));
        }
    }
}