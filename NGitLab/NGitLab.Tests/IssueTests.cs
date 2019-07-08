using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class IssueTests
    {
        private static User _currentUser;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _currentUser = Initialize.GitLabClient.Users.Current;

            Initialize.GitLabClient.Issues.Create(new IssueCreate
            {
                Id = Initialize.UnitTestProject.Id,
                Title = "Unassigned Test issue"
            });

            Initialize.GitLabClient.Issues.Create(new IssueCreate
            {
                Id = Initialize.UnitTestProject.Id,
                Title = "Assigned Test issue",
                AssigneeId = _currentUser.Id
            });
        }

        [Test]
        public void Test_get_issue_with_IssueQuery()
        {
            var issues = Initialize.GitLabClient.Issues.Get(new IssueQuery
            {
                State = IssueState.opened,
            }).ToList();

            Assert.AreNotEqual(0, issues.Count(),
                "The query retrieved all open issues, whether assigned or not");
            Assert.IsTrue(issues.Any(issue => issue.Assignee == null),
                "Some of the collected issues are unassigned");
            Assert.IsTrue(issues.Any(issue => issue.Assignee != null),
                "Some of the collected issues are assigned");
        }

        [Test]
        public void Test_get_unassigned_issues_with_IssueQuery()
        {
            var issues = Initialize.GitLabClient.Issues.Get(new IssueQuery
            {
                AssigneeId = 0, // = unassigned
                State = IssueState.opened,
            }).ToList();

            Assert.AreNotEqual(0, issues.Count(),
                "The query retrieved all open issues that are unassigned");
            Assert.IsTrue(issues.All(issue => issue.Assignee == null),
                "All collected issues are unassigned");
        }

        [Test]
        public void Test_get_assigned_issues_with_IssueQuery()
        {
            var issues = Initialize.GitLabClient.Issues.Get(new IssueQuery
            {
                AssigneeId = _currentUser.Id,
                State = IssueState.opened,
            }).ToList();

            Assert.AreNotEqual(0, issues.Count(),
                $"The query retrieved open issues that are assigned to the current user '{_currentUser.Username}'");
            Assert.IsTrue(issues.All(issue => issue.Assignee?.Username == _currentUser.Username),
                $"Collected issues are all assigned to the current user '{_currentUser.Username}'");
        }

        [Test]
        public void Test_get_assigned_issues_with_IssueQuery_and_project_id()
        {
            var issues = Initialize.GitLabClient.Issues.Get(Initialize.UnitTestProject.Id, new IssueQuery
            {
                AssigneeId = _currentUser.Id,
                State = IssueState.opened,
            }).ToList();

            Assert.AreNotEqual(0, issues.Count(),
                $"The query retrieved open issues that are assigned to the current user '{_currentUser.Username}'");
            Assert.IsTrue(issues.All(issue => issue.Assignee?.Username == _currentUser.Username),
                $"Collected issues are all assigned to the current user '{_currentUser.Username}'");
        }


        [Test]
        public void Test_get_issues_with_invalid_project_id_will_throw()
        {
            Assert.Throws<GitLabException>(() => Initialize.GitLabClient.Issues.ForProject(548975564).ToList());
            Assert.Throws<GitLabException>(() => Initialize.GitLabClient.Issues.Get(548975564, new IssueQuery()).ToList());
        }

        [Test]
        public void Test_get_all_project_issues()
        {
            var issues = Initialize.GitLabClient.Issues.ForProject(Initialize.UnitTestProject.Id).ToList();
            Assert.AreNotEqual(0, issues.Count);

            issues = Initialize.GitLabClient.Issues.Get(Initialize.UnitTestProject.Id, new IssueQuery()).ToList();
            Assert.AreNotEqual(0, issues.Count);
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