using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestClientTests
    {
        private IMergeRequestClient _mergeRequestClient;

        [SetUp]
        public void Setup()
        {
            _mergeRequestClient = Initialize.GitLabClient.GetMergeRequest(Initialize.UnitTestProject.Id);
        }

        [Test]
        public void Test_merge_request_api()
        {
            var branch = CreateBranch("my-super-feature");
            var mergeRequest = CreateMergeRequest(branch.Name, "master");
            Assert.AreEqual(mergeRequest.Id, _mergeRequestClient[mergeRequest.Iid].Id, "Test we can get a merge request by IId");

            ListMergeRequest(mergeRequest);
            UpdateMergeRequest(mergeRequest);
            AcceptMergeRequest(mergeRequest);
            var commits = _mergeRequestClient.Commits(mergeRequest.Iid).All;
            Assert.IsTrue(commits.Any(), "Can return the commits");
        }

        private void ListMergeRequest(Models.MergeRequest mergeRequest)
        {
            Assert.IsTrue(_mergeRequestClient.All.Any(x => x.Id == mergeRequest.Id), "Test 'All' accessor returns the merge request");
            Assert.IsTrue(_mergeRequestClient.AllInState(MergeRequestState.opened).Any(x => x.Id == mergeRequest.Id), "Can return all open requests");
            Assert.IsFalse(_mergeRequestClient.AllInState(MergeRequestState.merged).Any(x => x.Id == mergeRequest.Id), "Can return all closed requests");
        }

        private Models.MergeRequest CreateMergeRequest(string from, string to)
        {
            var mergeRequest = _mergeRequestClient.Create(new MergeRequestCreate
            {
                Title = "Merge my-super-feature into master",
                SourceBranch = from,
                TargetBranch = to,
                Labels = "a,b",
                RemoveSourceBranch = true
            });

            Assert.That(mergeRequest, Is.Not.Null);
            Assert.That(mergeRequest.Title, Is.EqualTo("Merge my-super-feature into master"));
            CollectionAssert.AreEqual(new[] { "a", "b" }, mergeRequest.Labels);
            Assert.That(mergeRequest.SourceBranch, Is.EqualTo(from));
            Assert.That(mergeRequest.TargetBranch, Is.EqualTo(to));
            Assert.AreEqual(null, mergeRequest.ShouldRemoveSourceBranch);
            Assert.AreEqual(mergeRequest.Squash, false);
            Assert.NotNull(mergeRequest.Sha);
            StringAssert.StartsWith(Initialize.GitLabHost, mergeRequest.WebUrl);
            Assert.AreEqual("can_be_merged", mergeRequest.MergeStatus);

            return mergeRequest;
        }

        private static Branch CreateBranch(string branchName)
        {
            var branch = Initialize.Repository.Branches.Create(new BranchCreate
            {
                Name = branchName,
                Ref = "master"
            });

            Initialize.Repository.Files.Create(new FileUpsert
            {
                RawContent = "test content",
                CommitMessage = "commit to merge",
                Branch = branch.Name,
                Path = $"mysuperfeature_from_{branchName}.txt",
            });

            return branch;
        }

        public void UpdateMergeRequest(Models.MergeRequest request)
        {
            var mergeRequest = _mergeRequestClient.Update(request.Iid, new MergeRequestUpdate
            {
                Title = "New title",
                Description = "New description",
                Labels = "a,b",
                SourceBranch = "my-super-feature",
                TargetBranch = "master",
            });

            Assert.AreEqual("New title", mergeRequest.Title);
            Assert.AreEqual("New description", mergeRequest.Description);
            Assert.IsFalse(mergeRequest.MergeWhenPipelineSucceeds);
            CollectionAssert.AreEqual(new[] { "a", "b" }, mergeRequest.Labels);
        }

        public void AcceptMergeRequest(Models.MergeRequest request)
        {
            var mergeRequest = _mergeRequestClient.Accept(
                mergeRequestIid: request.Iid,
                message: new MergeRequestAccept
                {
                    MergeCommitMessage = "Merge my-super-feature into master",
                    ShouldRemoveSourceBranch = true,
                });

            Assert.That(mergeRequest.State, Is.EqualTo(MergeRequestState.merged.ToString()));
            Assert.IsNull(Initialize.Repository.Branches.All.FirstOrDefault(x => x.Name == request.SourceBranch));
        }

        [Test]
        public void Test_gitlab_returns_an_error_when_trying_to_create_a_request_with_same_source_and_destination()
        {
            var exception = Assert.Throws<GitLabException>(() =>
            {
                _mergeRequestClient.Create(new MergeRequestCreate
                {
                    Title = "ErrorRequest",
                    SourceBranch = "master",
                    TargetBranch = "master"
                });
            });

            Assert.AreEqual("[\"You can not use same project/branch for source and target\"]", exception.ErrorMessage);
        }

        [Test]
        public void Test_merge_request_delete()
        {
            var branch = CreateBranch("tmp-branch-to-test-mr-deletion");
            var mergeRequest = CreateMergeRequest(branch.Name, "master");
            Assert.AreEqual(mergeRequest.Id, _mergeRequestClient[mergeRequest.Iid].Id, "Test can get a merge request by IId");

            Assert.DoesNotThrow(() =>
            {
                var mr = _mergeRequestClient[mergeRequest.Iid];
            });

            _mergeRequestClient.Delete(mergeRequest.Iid);

            Assert.Throws<GitLabException>(() =>
            {
                var mr = _mergeRequestClient[mergeRequest.Iid];
            });
        }

        [Test]
        public void Test_merge_request_approvers()
        {
            var branch = CreateBranch("tmp-branch-to-test-mr-approvers");
            var mergeRequest = CreateMergeRequest(branch.Name, "master");

            var approvalClient = _mergeRequestClient.ApprovalClient(mergeRequest.Iid);
            var approvers = approvalClient.Approvals.Approvers;

            Assert.AreEqual(0, approvers.Length, "Initially no approver defined");

            // --- Add the "current user" as approver for this MR ---

            var users = Initialize.GitLabClient.Users;            

            var approversChange = new MergeRequestApproversChange()
            {
                Approvers = new[] { users.Current.Id }
            };

            approvalClient.ChangeApprovers(approversChange);

            approvers = approvalClient.Approvals.Approvers;

            Assert.AreEqual(1, approvers.Length, "A single approver defined");
            Assert.AreEqual(users.Current.Id, approvers[0].User.Id, "The approver is the current user");
        }
    }
}
