using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class CommitsTests
    {
        [Test]
        public async Task Test_can_get_commit()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);

            var commit = context.Client.GetCommits(project.Id).GetCommit(project.DefaultBranch);
            Assert.IsNotNull(commit.Message);
            Assert.IsNotNull(commit.ShortId);
        }

        [Test]
        public async Task Test_can_get_stats_in_commit()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            context.Client.GetRepository(project.Id).Files.Create(new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "file to be updated",
                Path = "CommitStats.txt",
                RawContent = "I'm defective and i need to be fixeddddddddddddddd",
            });

            context.Client.GetRepository(project.Id).Files.Update(new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "fixing the file",
                Path = "CommitStats.txt",
                RawContent = "I'm no longer defective and i have been fixed\n\n\r\n\r\rEnjoy",
            });

            var commit = context.Client.GetCommits(project.Id).GetCommit(project.DefaultBranch);
            Assert.AreEqual(4, commit.Stats.Additions);
            Assert.AreEqual(1, commit.Stats.Deletions);
            Assert.AreEqual(5, commit.Stats.Total);
        }
    }
}
