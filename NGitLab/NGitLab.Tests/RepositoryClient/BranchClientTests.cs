using System;
using System.Linq;
using System.Net;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class BranchClientTests
    {
        private IBranchClient _branches;

        [SetUp]
        public void Setup()
        {
            var project = Initialize.GitLabClient.Projects.Owned.First();
            _branches = Initialize.GitLabClient.GetRepository(project.Id).Branches;
        }

        [Test]
        public void GetAll()
        {
            CollectionAssert.IsNotEmpty(_branches.All.ToArray());
        }

        [Test]
        public void GetByName()
        {
            var branch = _branches["master"];
            Assert.IsNotNull(branch);
            Assert.IsNotNull(branch.Name);
        }

        [Test]
        public void AddDelete()
        {
            const string branchName = "merge-me-to-master";

            _branches.Create(new BranchCreate
            {
                Name = branchName,
                Ref = "master"
            });

            Assert.IsNotNull(_branches[branchName]);

            var result = _branches.Delete(branchName);
            Assert.AreEqual(branchName, result.Name);

            AssertCannotFind(() => _branches[branchName]);
        }

        private static void AssertCannotFind<T>(Func<T> get)
        {
            try
            {
                var dummyResult = get();
                Assert.Fail($"Not supposed to return {dummyResult}, should throw a 404 exception instead.");
            }
            catch (GitLabException ex)
            {
                Assert.IsTrue(ex.StatusCode == HttpStatusCode.NotFound);
            }
        }
    }
}