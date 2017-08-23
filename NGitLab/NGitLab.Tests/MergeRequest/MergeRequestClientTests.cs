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
        [Order(2)]
        public void UpdateMergeRequest() {
            var mergeRequest = mergeRequestClient.AllInState(MergeRequestState.opened).FirstOrDefault(x => x.Title == "Merge my-super-feature into master");
            
            var updatedMergeRequest = mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate {
                Title = "Merge my-super-feature into master updated",
            });

            Assert.That(updatedMergeRequest, Is.Not.Null);
            updatedMergeRequest.Iid.ShouldBe(mergeRequest.Iid);
            updatedMergeRequest.Title.ShouldBe("Merge my-super-feature into master updated");
            updatedMergeRequest.State.ShouldBe(MergeRequestState.opened);
        }
        [Test]
        [Category("Server_Required")]
        [Order(3)]
        public void CloseMergeRequest() {
            var mergeRequest = mergeRequestClient.AllInState(MergeRequestState.opened).FirstOrDefault(x => x.Title == "Merge my-super-feature into master updated");
            
            var updatedMergeRequest = mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate {
                NewState = MergeRequestUpdateState.close,
            });
            Assert.That(updatedMergeRequest, Is.Not.Null);
            updatedMergeRequest.Iid.ShouldBe(mergeRequest.Iid);
            updatedMergeRequest.State.ShouldBe(MergeRequestState.closed);
        }
        [Test]
        [Category("Server_Required")]
        [Order(3)]
        [Ignore("todo")]
        public void AcceptMergeRequest() {
            var mergeRequest = mergeRequestClient.AllInState(MergeRequestState.opened).FirstOrDefault(x => x.Title == "Merge my-super-feature into master updated");
            var updatedMergeRequest = mergeRequestClient.Accept(
                mergeRequest.Iid,
                new MergeCommitMessage {Message = "Merge my-super-feature into master"});

            updatedMergeRequest.State.ShouldBe(MergeRequestState.merged);
        }
    }
}