using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestClientTests
    {
        private IMergeRequestClient _mergeRequest;

        [SetUp]
        public void Setup()
        {
            _mergeRequest = Initialize.GitLabClient.GetMergeRequest(Initialize.UnitTestProject.Id);
        }

        [Test]
        public void Test_merge_request_api()
        {
            var mergeRequest = CreateMergeRequest();
            Assert.AreEqual(mergeRequest.Id, _mergeRequest[mergeRequest.Iid].Id, "Test can get a merge request by IId");

            ListMergeRequest(mergeRequest);
            UpdateMergeRequest(mergeRequest);
            AcceptMergeRequest(mergeRequest);
        }

        private void ListMergeRequest(Models.MergeRequest mergeRequest)
        {
            Assert.IsTrue(_mergeRequest.All.Any(x => x.Id == mergeRequest.Id), "Test all accessor returns the merge request");
            Assert.IsTrue(_mergeRequest.AllInState(MergeRequestState.opened).Any(x => x.Id == mergeRequest.Id), "Can return all open request");
            Assert.IsFalse(_mergeRequest.AllInState(MergeRequestState.merged).Any(x => x.Id == mergeRequest.Id), "Can return all closed request");
        }

        private Models.MergeRequest CreateMergeRequest()
        {
            var branch = CreateBranch();
            var mergeRequest = _mergeRequest.Create(new MergeRequestCreate
            {
                Title = "Merge my-super-feature into master",
                SourceBranch = branch.Name,
                TargetBranch = "master"
            });

            Assert.That(mergeRequest, Is.Not.Null);
            Assert.That(mergeRequest.Title, Is.EqualTo("Merge my-super-feature into master"));
            Assert.That(mergeRequest.SourceBranch, Is.EqualTo("my-super-feature"));
            Assert.That(mergeRequest.TargetBranch, Is.EqualTo("master"));

            return mergeRequest;
        }

        private static Branch CreateBranch()
        {
            var branch = Initialize.Repository.Branches.Create(new BranchCreate
            {
                Name = "my-super-feature",
                Ref = "master"
            });

            Initialize.Repository.Files.Create(new FileUpsert
            {
                RawContent = "test content",
                CommitMessage = "commit to merge",
                Branch = branch.Name,
                Path = "mysuperfeature.txt",
            });

            return branch;
        }

        public void UpdateMergeRequest(Models.MergeRequest request)
        {
            var mergeRequest = _mergeRequest.Update(request.Iid, new MergeRequestUpdate
            {
                Title = "New title",
                SourceBranch = "my-super-feature",
                TargetBranch = "master",
            });

            Assert.AreEqual("New title", mergeRequest.Title);
        }

        public void AcceptMergeRequest(Models.MergeRequest request)
        {
            var mergeRequest = _mergeRequest.Accept(
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
                _mergeRequest.Create(new MergeRequestCreate
                {
                    Title = "ErrorRequest",
                    SourceBranch = "master",
                    TargetBranch = "master"
                });
            });

            Assert.AreEqual("[\"You can not use same project/branch for source and target\"]", exception.ErrorMessage);
        }
    }
}