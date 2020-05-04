using System;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class BranchClientTests
    {
        private IBranchClient _branchClient;

        [SetUp]
        public void Setup()
        {
            _branchClient = Initialize.Repository.Branches;
        }

        [Test]
        public void Test_CommitInfoIsCorrectlyDeserialized()
        {
            var masterBranch = _branchClient["master"];
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

            Assert.AreEqual("robot", commit.AuthorName);
            Assert.AreEqual("robot@gitlab.example.com", commit.AuthorEmail);
            Assert.Less(fiveMinutesAgo, commit.AuthoredDate);

            Assert.AreEqual("robot", commit.CommitterName);
            Assert.AreEqual("robot@gitlab.example.com", commit.CommitterEmail);
            Assert.Less(fiveMinutesAgo, commit.CommittedDate);

            Assert.IsTrue(Uri.TryCreate(commit.WebUrl, UriKind.Absolute, out _));
        }
    }
}
