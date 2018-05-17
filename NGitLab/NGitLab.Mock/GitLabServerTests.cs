using NUnit.Framework;
using NUnit.Framework.Internal;

namespace NGitLab.Mock
{
    [TestFixture]
    public class GitLabServerTests
    {
        [Test]
        public void Test_commit_creates_another_commit()
        {
            var server = new GitLabServer();
            var project = server.CreateProject();

            var firstCommit = project.Commit("Test");
            var secondCommit = project.Commit("Test");

            Assert.AreNotEqual(firstCommit.Id, secondCommit.Id);
            Assert.AreEqual(2, project.Commits.Count);
        }

        [Test]
        public void Test_sha1_are_deterministic()
        {
            var server = new GitLabServer();
            var projectA = server.CreateProject();
            var projectB = server.CreateProject();

            var firstCommit = projectA.Commit("Test");
            var secondCommit = projectB.Commit("Test");

            Assert.AreEqual(firstCommit.Id, secondCommit.Id);
        }
    }
}