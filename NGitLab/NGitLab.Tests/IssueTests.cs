using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class IssueTests
    {
        [OneTimeSetUp]
        public void FixtureSetup()
        {
            CreateIssue();
        }

        private void CreateIssue()
        {
            Initialize.GitLabClient.Issues.Create(new IssueCreate
            {
                Id = Initialize.UnitTestProject.Id,
                Title = "New Test issue"
            });
        }

        [Test]
        public void Test_get_issue_with_IssueQuery()
        {
            var issues = Initialize.GitLabClient.Issues.Get(new IssueQuery
            {
                State = IssueState.opened,
            });

            Assert.AreNotEqual(0, issues.Count());
        }

        [Test]
        public void Test_post_commit_status()
        {
            var issue = Initialize.GitLabClient.Issues.Create(new IssueCreate
            {
                Id = Initialize.UnitTestProject.Id,
                Title = "New Test issue 2"
            });

            Assert.IsNotNull(issue);
        }
    }
}