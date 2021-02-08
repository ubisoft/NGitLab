using System.Threading.Tasks;
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

            var commit = context.Client.GetCommits(project.Id).GetCommit("master");
            Assert.IsNotNull(commit.Message);
            Assert.IsNotNull(commit.ShortId);
        }
    }
}
