using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.MergeRequest {
    public class MergeRequestClientTests {
        readonly IMergeRequestClient mergeRequestClient;

        public MergeRequestClientTests() {
            mergeRequestClient = _MergeRequestClientTests.MergeRequestClient;
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllMergeRequests() {
            mergeRequestClient.All().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllMergeRequestsInCertainState() {
            mergeRequestClient.AllInState(MergeRequestState.opened).ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetMergeRequestById() {
            var mergeRequest = mergeRequestClient.AllInState(MergeRequestState.opened).First();
            mergeRequestClient.Get(mergeRequest.Iid).ShouldNotBeNull();
        }

        [Test]
        [Category("Server_Required")]
        [Order(1)]
        public void CreateMergeRequest() {
            var mergeRequest = mergeRequestClient.Create(new MergeRequestCreate {
                Title = "Merge my-super-feature into master",
                SourceBranch = "my-super-feature",
                TargetBranch = "master"
            });

            Assert.That(mergeRequest, Is.Not.Null);
        }

        [Test]
        [Category("Server_Required")]
        public void UpdateMergeRequest() {
            var mergeRequest = mergeRequestClient.Update(5, new MergeRequestUpdate {
                Title = "Merge my-super-feature into master",
                TargetBranch = "my-super-feature",
                SourceBranch = "master",
                NewState = MergeRequestStateEvent.reopen
            });

            Assert.That(mergeRequest, Is.Not.Null);
        }

        [Test]
        [Category("Server_Required")]
        public void AcceptMergeRequest() {
            var mergeRequest = mergeRequestClient.Accept(
                6,
                new MergeCommitMessage {Message = "Merge my-super-feature into master"});

            Assert.That(mergeRequest.State, Is.EqualTo(MergeRequestState.merged.ToString()));
        }
    }
}