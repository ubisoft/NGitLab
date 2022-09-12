using System;
using System.Linq;
using System.Net;
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

        [TestCase(false)]
        [TestCase(true)]
        public void Test_merge_request_with_no_rebase_required_can_be_accepted(bool sourceProjectSameAsTargetProject)
        {
            // Arrange
            using var server = new GitLabServer();

            var contributor = server.Users.AddNew("contributor");
            var maintainer = server.Users.AddNew("maintainer");

            var targetGroup = new Group("TheTargetGroup");
            server.Groups.Add(targetGroup);

            var targetProject = new Project("TheTargetProject") { Visibility = VisibilityLevel.Internal };
            targetGroup.Projects.Add(targetProject);

            targetProject.Permissions.Add(new Permission(maintainer, AccessLevel.Maintainer));
            targetProject.Repository.Commit(maintainer, "A commit");

            var sourceProject = sourceProjectSameAsTargetProject ?
                targetProject :
                targetProject.Fork(contributor.Namespace, contributor, "TheSourceProject");
            sourceProject.Repository.CreateAndCheckoutBranch("to-be-merged");
            sourceProject.Repository.Commit(contributor, "add a file", new[] { File.CreateFromText(Guid.NewGuid().ToString("N"), "This is the new file's content") });

            var mr = targetProject.CreateMergeRequest(contributor, "A great title", "A great description", targetProject.DefaultBranch, "to-be-merged", sourceProject);
            mr.Assignee = new UserRef(maintainer);

            targetProject.MergeMethod = "ff";

            var maintainerClient = server.CreateClient("maintainer");

            // Act
            var modelMr = maintainerClient.GetMergeRequest(mr.Project.Id).Accept(mr.Iid, new MergeRequestMerge
            {
                MergeWhenPipelineSucceeds = mr.HeadPipeline != null,
                ShouldRemoveSourceBranch = true,
            });

            // Assert
            Assert.IsFalse(modelMr.HasConflicts);
            Assert.IsNotNull(modelMr.DiffRefs?.BaseSha);
            Assert.AreEqual(modelMr.DiffRefs.BaseSha, modelMr.DiffRefs.StartSha);
            Assert.AreEqual("merged", modelMr.State);

            Assert.IsFalse(targetProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)),
                "Since the merge succeeded and 'ShouldRemoveSourceBranch' was set, 'to-be-merged' branch should be gone");

            Assert.IsFalse(sourceProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)),
                "Since the merge succeeded and 'ShouldRemoveSourceBranch' was set, 'to-be-merged' branch should be gone");
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Test_merge_request_with_non_conflicting_rebase_needed_and_merge_method_ff_cannot_be_accepted(bool sourceProjectSameAsTargetProject)
        {
            // Arrange
            using var server = new GitLabServer();

            var contributor = server.Users.AddNew("contributor");
            var maintainer = server.Users.AddNew("maintainer");

            var targetGroup = new Group("TheTargetGroup");
            server.Groups.Add(targetGroup);

            var targetProject = new Project("TheTargetProject") { Visibility = VisibilityLevel.Internal };
            targetGroup.Projects.Add(targetProject);

            targetProject.Permissions.Add(new Permission(maintainer, AccessLevel.Maintainer));
            targetProject.Repository.Commit(maintainer, "A commit");

            var sourceProject = sourceProjectSameAsTargetProject ?
                targetProject :
                targetProject.Fork(contributor.Namespace, contributor, "TheSourceProject");
            sourceProject.Repository.CreateAndCheckoutBranch("to-be-merged");
            sourceProject.Repository.Commit(contributor, "add a file", new[] { File.CreateFromText(Guid.NewGuid().ToString("N"), "This is the new file's content") });

            targetProject.MergeMethod = "ff";

            targetProject.Repository.Checkout(targetProject.DefaultBranch);
            targetProject.Repository.Commit(maintainer, "add a file", new[] { File.CreateFromText(Guid.NewGuid().ToString("N"), "This is the new file's content") });

            var mr = targetProject.CreateMergeRequest(contributor, "A great title", "A great description", targetProject.DefaultBranch, "to-be-merged", sourceProject);
            mr.Assignee = new UserRef(maintainer);

            var maintainerClient = server.CreateClient("maintainer");

            // Act/Assert
            var exception = Assert.Throws<GitLabException>(() => maintainerClient.GetMergeRequest(mr.Project.Id).Accept(mr.Iid, new MergeRequestMerge
            {
                MergeWhenPipelineSucceeds = mr.HeadPipeline != null,
                ShouldRemoveSourceBranch = true,
            }));
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, exception.StatusCode);
            Assert.IsTrue(exception.Message.Equals("The MR cannot be merged with method 'ff': the source branch must first be rebased", StringComparison.Ordinal));

            Assert.IsFalse(mr.HasConflicts);
            Assert.AreNotEqual(mr.BaseSha, mr.StartSha);

            Assert.IsTrue(targetProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)),
                "Since the merge failed, 'to-be-merged' branch should still be there");

            Assert.IsTrue(sourceProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)),
                "Since the merge failed, 'to-be-merged' branch should still be there");
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Test_merge_request_with_conflicts_cannot_be_accepted(bool sourceProjectSameAsTargetProject)
        {
            // Arrange
            using var server = new GitLabServer();

            var contributor = server.Users.AddNew("contributor");
            var maintainer = server.Users.AddNew("maintainer");

            var targetGroup = new Group("TheTargetGroup");
            server.Groups.Add(targetGroup);

            var targetProject = new Project("TheTargetProject") { Visibility = VisibilityLevel.Internal };
            targetGroup.Projects.Add(targetProject);

            targetProject.Permissions.Add(new Permission(maintainer, AccessLevel.Maintainer));
            targetProject.Repository.Commit(maintainer, "A commit");

            var sourceProject = sourceProjectSameAsTargetProject ?
                targetProject :
                targetProject.Fork(contributor.Namespace, contributor, "TheSourceProject");
            var conflictingFile = Guid.NewGuid().ToString("N");
            sourceProject.Repository.CreateAndCheckoutBranch("to-be-merged");
            sourceProject.Repository.Commit(contributor, "add a file", new[] { File.CreateFromText(conflictingFile, "This is the new file's content") });

            targetProject.MergeMethod = "ff";

            targetProject.Repository.Checkout(targetProject.DefaultBranch);
            targetProject.Repository.Commit(maintainer, "add a file", new[] { File.CreateFromText(conflictingFile, "This is conflicting content") });

            var mr = targetProject.CreateMergeRequest(contributor, "A great title", "A great description", targetProject.DefaultBranch, "to-be-merged", sourceProject);
            mr.Assignee = new UserRef(maintainer);

            var maintainerClient = server.CreateClient("maintainer");

            // Act/Assert
            var exception = Assert.Throws<GitLabException>(() => maintainerClient.GetMergeRequest(mr.Project.Id).Accept(mr.Iid, new MergeRequestMerge
            {
                MergeWhenPipelineSucceeds = mr.HeadPipeline != null,
                ShouldRemoveSourceBranch = true,
            }));
            Assert.AreEqual(HttpStatusCode.NotAcceptable, exception.StatusCode);
            Assert.IsTrue(exception.Message.Equals("The merge request has some conflicts and cannot be merged", StringComparison.Ordinal));

            Assert.IsTrue(mr.HasConflicts);
            Assert.AreNotEqual(mr.BaseSha, mr.StartSha);

            Assert.IsTrue(targetProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)),
                "Since the merge failed, 'to-be-merged' branch should still be there");

            Assert.IsTrue(sourceProject.Repository.GetAllBranches().Any(b => b.FriendlyName.EndsWith("to-be-merged", StringComparison.Ordinal)),
                "Since the merge failed, 'to-be-merged' branch should still be there");
        }
    }
}
