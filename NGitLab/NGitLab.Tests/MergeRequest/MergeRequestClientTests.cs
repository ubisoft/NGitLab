using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestClientTests
    {
        private readonly IMergeRequestClient _mergeRequest;

        public MergeRequestClientTests()
        {
            _mergeRequest = _MergeRequestClientTests.MergeRequestClient;
        }

        [Test]
        public void GetAllMergeRequests()
        {
            var mergeRequests = _mergeRequest.All.ToArray();
            CollectionAssert.IsNotEmpty(mergeRequests);
        }

        [Test]
        public void GetAllMergeRequestsInCertainState()
        {
            var mergeRequests = _mergeRequest.AllInState(MergeRequestState.opened).ToArray();
            CollectionAssert.IsNotEmpty(mergeRequests);
            CollectionAssert.AreEqual(Enumerable.Repeat("opened", 2), mergeRequests.Select(x => x.State));
        }

        [Test]
        public void GetMergeRequestById()
        {
            const int mergeReqestId = 1;
            Assert.AreEqual(mergeReqestId, _mergeRequest[mergeReqestId].Id);
        }

        [Test]
        [Ignore(reason: "WIP")]
        public void CreateMergeRequest()
        {
            var mergeRequest = _mergeRequest.Create(new MergeRequestCreate
            {
                Title = "Merge my-super-feature into master",
                SourceBranch = "my-super-feature",
                TargetBranch = "master"
            });

            Assert.That(mergeRequest, Is.Not.Null);
            Assert.That(mergeRequest.Title, Is.EqualTo("Merge my-super-feature into master"));
            Assert.That(mergeRequest.SourceBranch, Is.EqualTo("my-super-feature"));
            Assert.That(mergeRequest.TargetBranch, Is.EqualTo("master"));
        }

        [Test]
        public void UpdateMergeRequest()
        {
            var mergeRequest = _mergeRequest.Update(5, new MergeRequestUpdate
            {
                Title = "Merge my-super-feature into master",
                TargetBranch = "my-super-feature",
                SourceBranch = "master",
                NewState = MergeRequestStateEvent.close.ToString()
            });

            Assert.That(mergeRequest, Is.Not.Null);
        }

        [Test]
        public void AcceptMergeRequest()
        {
            var mergeRequest = _mergeRequest.Accept(
                mergeRequestId: 4,
                message: new MergeCommitMessage {Message = "Merge my-super-feature into master"});

            Assert.That(mergeRequest.State, Is.EqualTo(MergeRequestState.merged.ToString()));
        }
    }
}