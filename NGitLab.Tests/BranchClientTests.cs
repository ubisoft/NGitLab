using System;
using System.Threading.Tasks;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class BranchClientTests
    {
        [Test]
        public async Task Test_CommitInfoIsCorrectlyDeserialized()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var branchClient = context.Client.GetRepository(project.Id).Branches;
            var currentUser = context.Client.Users.Current;

            var masterBranch = branchClient[project.DefaultBranch];
            Assert.NotNull(masterBranch);

            var commit = masterBranch.Commit;
            Assert.NotNull(commit);

            Assert.AreEqual(40, commit.Id.ToString().Length);
            Assert.LessOrEqual(7, commit.ShortId.Length);

            var fiveMinutesAgo = DateTime.UtcNow - TimeSpan.FromMinutes(5);
            Assert.Less(fiveMinutesAgo, commit.CreatedAt);

            Assert.LessOrEqual(1, commit.Parents.Length);

            Assert.AreEqual("add test file 2", commit.Title);
            Assert.AreEqual("add test file 2", commit.Message);

            Assert.AreEqual(currentUser.Name, commit.AuthorName);
            Assert.AreEqual(currentUser.Email, commit.AuthorEmail);
            Assert.Less(fiveMinutesAgo, commit.AuthoredDate);

            Assert.AreEqual(currentUser.Name, commit.CommitterName);
            Assert.AreEqual(currentUser.Email, commit.CommitterEmail);
            Assert.Less(fiveMinutesAgo, commit.CommittedDate);

            Assert.IsTrue(Uri.TryCreate(commit.WebUrl, UriKind.Absolute, out _));
        }
    }
}
