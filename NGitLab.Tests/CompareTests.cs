using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class CompareTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_compare_equal()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var compareResults = context.Client.GetRepository(project.Id).Compare(new CompareQuery(project.DefaultBranch, project.DefaultBranch));

            Assert.IsNotNull(compareResults);
            Assert.AreEqual(0, compareResults.Commits.Length);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_compare()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);

            var devTestBranchCreate = new BranchCreate();
            devTestBranchCreate.Ref = project.DefaultBranch;
            devTestBranchCreate.Name = "devtest";

            context.Client.GetRepository(project.Id).Branches.Create(devTestBranchCreate);

            context.Client.GetRepository(project.Id).Files.Create(new FileUpsert
            {
                Branch = "devtest",
                CommitMessage = "file to be compared",
                Path = "compare1.txt",
                RawContent = "compare me",
            });

            context.Client.GetRepository(project.Id).Files.Create(new FileUpsert
            {
                Branch = "devtest",
                CommitMessage = "file to be compared, too",
                Path = "compare2.txt",
                RawContent = "compare me now",
            });

            var compareResults = context.Client.GetRepository(project.Id).Compare(new CompareQuery(project.DefaultBranch, "devtest"));

            Assert.IsNotNull(compareResults);
            Assert.AreEqual(2, compareResults.Commits.Length);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_compare_invalid()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);

            Assert.Catch<GitLabException>(() =>
            {
                context.Client.GetRepository(project.Id).Compare(new CompareQuery(project.DefaultBranch, "testblub"));
            }, "404 Ref Not Found", null);
        }
    }
}
