using System;
using System.IO;
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
            Assert.IsTrue(branch.Default);
        }

        [Test]
        public void AddDelete()
        {
            var branchName = $"merge-me-to-master_{Path.GetRandomFileName()}";

            _branches.Create(new BranchCreate
            {
                Name = branchName,
                Ref = "master"
            });

            var branch = _branches[branchName];
            Assert.IsNotNull(branch);
            Assert.IsFalse(branch.Default);

            _branches.Delete(branchName);

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