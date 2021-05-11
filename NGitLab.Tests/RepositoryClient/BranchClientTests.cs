using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class BranchClientTests
    {
        [Test]
        public async Task GetAll()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var branches = context.Client.GetRepository(project.Id).Branches;

            CollectionAssert.IsNotEmpty(branches.All.ToArray());
        }

        [Test]
        public async Task GetByName()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var branches = context.Client.GetRepository(project.Id).Branches;

            var branch = branches[project.DefaultBranch];

            Assert.IsNotNull(branch);
            Assert.IsNotNull(branch.Name);
            Assert.IsTrue(branch.Default);
        }

        [Test]
        public async Task AddDelete()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var branches = context.Client.GetRepository(project.Id).Branches;

            var branchName = $"merge-me-to-{project.DefaultBranch}_{Path.GetRandomFileName()}";

            branches.Create(new BranchCreate
            {
                Name = branchName,
                Ref = project.DefaultBranch,
            });

            var branch = branches[branchName];
            Assert.IsNotNull(branch);
            Assert.IsFalse(branch.Default);

            branches.Protect(branchName);

            branches.Unprotect(branchName);

            branches.Delete(branchName);

            AssertCannotFind(() => branches[branchName]);
        }

        [Test]
        public async Task Test_that_branch_names_containing_slashes_are_supported()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var branches = context.Client.GetRepository(project.Id).Branches;

            var branchName = "feature/addNewStuff/toto";

            branches.Create(new BranchCreate
            {
                Name = branchName,
                Ref = project.DefaultBranch,
            });

            Assert.IsNotNull(branches[branchName]);

            branches.Protect(branchName);

            branches.Unprotect(branchName);

            branches.Delete(branchName);

            AssertCannotFind(() => branches[branchName]);
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
                Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            }
        }
    }
}
