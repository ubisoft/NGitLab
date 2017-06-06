using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

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
        [Category("Server_Required")]
        public void GetAllMergeRequests()
        {
            _mergeRequest.All().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllMergeRequestsInCertainState()
        {
            _mergeRequest.AllInState(MergeRequestState.opened).ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetMergeRequestById()
        {
            const int mergeReqestId = 5;
            Assert.AreEqual(mergeReqestId, _mergeRequest.Get(mergeReqestId).Id);
        }

        [Test]
        [Category("Server_Required")]
        public void CreateMergeRequest()
        {
            var mergeRequest = _mergeRequest.Create(new MergeRequestCreate
            {
                Title = "Merge my-super-feature into master",
                SourceBranch = "my-super-feature",
                TargetBranch = "master"
            });

            Assert.That(mergeRequest, Is.Not.Null);
        }

        [Test]
        [Category("Server_Required")]
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
        [Category("Server_Required")]
        public void AcceptMergeRequest()
        {
            var mergeRequest = _mergeRequest.Accept(
                mergeRequestId: 6,
                message: new MergeCommitMessage { Message = "Merge my-super-feature into master" });

            Assert.That(mergeRequest.State, Is.EqualTo(MergeRequestState.merged.ToString()));
        }
    }
}