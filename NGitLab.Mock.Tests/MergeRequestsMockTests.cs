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
                .WithUser("user1", asDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithMergeRequest("Merge request 1", "branch-01", author: "user1", assignee: "user2")
                    .WithMergeRequest("Merge request 2", "branch-02", author: "user2", assignee: "user1"))
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
                .WithUser("user1", asDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithMergeRequest("Merge request 1", "branch-01", author: "user1", assignee: "user2")
                    .WithMergeRequest("Merge request 2", "branch-02", author: "user2", assignee: "user1"))
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
                .WithUser("user1", asDefault: true)
                .WithUser("user2")
                .WithProject("Test", configure: project => project
                    .WithMergeRequest("Merge request 1", "branch-01", author: "user1", approvers: new[] { "user2" })
                    .WithMergeRequest("Merge request 2", "branch-02", author: "user2", approvers: new[] { "user1" }))
                .BuildServer();

            var client = server.CreateClient("user1");
            var mergeRequests = client.MergeRequests.Get(new MergeRequestQuery { ApproverIds = new[] { 1 } }).ToArray();

            Assert.AreEqual(1, mergeRequests.Length, "Merge requests count is invalid");
            Assert.AreEqual("Merge request 2", mergeRequests[0].Title, "Merge request found is invalid");
        }
    }
}
