using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class IssuesMockTests
    {
        [Test]
        public void Test_issues_created_by_me_can_be_listed()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var user2 = new User("user2");
            gitLabServer.Users.Add(user2);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            var issue1 = new Issue { Author = new UserRef(user1), Assignee = new UserRef(user2), Title = "Issue 1" };
            project.Issues.Add(issue1);
            var issue2 = new Issue { Author = new UserRef(user2), Assignee = new UserRef(user1), Title = "Issue 2" };
            project.Issues.Add(issue2);

            var client = gitLabServer.CreateClient(user1);
            var issues = client.Issues.Get(new IssueQuery { Scope = "created_by_me" }).ToArray();

            Assert.AreEqual(1, issues.Length, "Issues count is invalid");
            Assert.AreEqual("Issue 1", issues[0].Title, "Issue found is invalid");
        }

        [Test]
        public void Test_issues_assigned_to_me_can_be_listed()
        {
            using var gitLabServer = new GitLabServer();
            var user1 = new User("user1");
            gitLabServer.Users.Add(user1);
            var user2 = new User("user2");
            gitLabServer.Users.Add(user2);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            var issue1 = new Issue { Author = new UserRef(user1), Assignee = new UserRef(user2), Title = "Issue 1" };
            project.Issues.Add(issue1);
            var issue2 = new Issue { Author = new UserRef(user2), Assignee = new UserRef(user1), Title = "Issue 2" };
            project.Issues.Add(issue2);

            var client = gitLabServer.CreateClient(user1);
            var issues = client.Issues.Get(new IssueQuery { Scope = "assigned_to_me" }).ToArray();

            Assert.AreEqual(1, issues.Length, "Issues count is invalid");
            Assert.AreEqual("Issue 2", issues[0].Title, "Issue found is invalid");
        }

        [Test]
        public void Test_issues_assignee_not_throwing_when_assignees_is_null()
        {
            using var gitLabServer = new GitLabServer();
            var user = new User("user");
            gitLabServer.Users.Add(user);
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);
            var issue1 = new Issue { Author = new UserRef(user), Assignee = null, Title = "Issue title" };
            project.Issues.Add(issue1);

            var client = gitLabServer.CreateClient(user);
            Assert.DoesNotThrow(() => client.Issues.Get(new IssueQuery { Scope = "assigned_to_me" }).ToArray());
        }
    }
}
