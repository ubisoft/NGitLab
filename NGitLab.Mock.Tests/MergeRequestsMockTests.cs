using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class MergeRequestsMockTests
    {
        [Test]
        public void Test_merge_requests_created_by_me_can_be_listed()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2")
                    .WithMergeRequest("branch-02", title: "Merge request 2", author: "user2", assignee: "user1"))
                .BuildServer();

            var client = server.CreateClient("user1");
            var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { Scope = "created_by_me" }).ToArray();

            Assert.AreEqual(1, mergeRequests.Length, "Merge requests count is invalid");
            Assert.AreEqual("Merge request 1", mergeRequests[0].Title, "Merge request found is invalid");
        }

        [Test]
        public void Test_merge_requests_assigned_to_me_can_be_listed()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", assignee: "user2")
                    .WithMergeRequest("branch-02", title: "Merge request 2", author: "user2", assignee: "user1"))
                .BuildServer();

            var client = server.CreateClient("user1");
            var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { Scope = "assigned_to_me" }).ToArray();

            Assert.AreEqual(1, mergeRequests.Length, "Merge requests count is invalid");
            Assert.AreEqual("Merge request 2", mergeRequests[0].Title, "Merge request found is invalid");
        }

        [Test]
        public void Test_merge_requests_approvable_by_me_can_be_listed()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithMergeRequest("branch-01", title: "Merge request 1", author: "user1", approvers: new[] { "user2" })
                    .WithMergeRequest("branch-02", title: "Merge request 2", author: "user2", approvers: new[] { "user1" }))
                .BuildServer();

            var client = server.CreateClient("user1");
            var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { ApproverIds = new[] { 1 } }).ToArray();

            Assert.AreEqual(1, mergeRequests.Length, "Merge requests count is invalid");
            Assert.AreEqual("Merge request 2", mergeRequests[0].Title, "Merge request found is invalid");
        }

        [Test]
        public void Test_merge_requests_can_be_listed_when_assignee_not_set()
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
            var mergeRequest1 = new MergeRequest { Author = new UserRef(user1), Title = "Merge request 1", SourceProject = project };
            project.MergeRequests.Add(mergeRequest1);
            var mergeRequest2 = new MergeRequest { Author = new UserRef(user2), Assignee = new UserRef(user1), Title = "Merge request 2", SourceProject = project };
            project.MergeRequests.Add(mergeRequest2);

            var client = gitLabServer.CreateClient(user1);
            var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { Scope = "assigned_to_me" }).ToArray();

            Assert.AreEqual(1, mergeRequests.Length, "Merge requests count is invalid");
            Assert.AreEqual("Merge request 2", mergeRequests[0].Title, "Merge request found is invalid");
        }

        [Test]
        public void Test_merge_requests_assignee_should_update_assignees_and_vice_versa()
        {
            var user1 = new User("user1");
            var user2 = new User("user2");

            var mergeRequestSingle = new MergeRequest
            {
                Assignee = new UserRef(user1),
            };

            var mergeRequestTwo = new MergeRequest
            {
                Assignees = new[] { new UserRef(user1), new UserRef(user2) },
            };

            Assert.AreEqual(1, mergeRequestSingle.Assignees.Count, "Merge request assignees count invalid");
            Assert.AreEqual("user1", mergeRequestTwo.Assignee.UserName, "Merge request assignee is invalid");
        }
    }
}
