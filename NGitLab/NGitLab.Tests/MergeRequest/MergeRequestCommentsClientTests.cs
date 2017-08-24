using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.MergeRequest {
    public class MergeRequestCommentsClientTests {
        readonly IMergeRequestCommentClient mergeRequestComments;

        public MergeRequestCommentsClientTests() {
            var mergeRequestClient = _MergeRequestClientTests.MergeRequestClient;
            var mergeRequest = mergeRequestClient.AllInState(MergeRequestState.opened).FirstOrDefault(x => x.Title == "mergeme");
            mergeRequestComments = _MergeRequestClientTests.MergeRequestClient.Comments(mergeRequest.Iid);
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllComments() {
            mergeRequestComments.All.ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void AddCommentToMergeRequest() {
            const string commentMessage = "note";
            var newComment = new MergeRequestComment {Note = commentMessage};

            var mergeRequest = mergeRequestComments.Add(newComment);

            Assert.That(mergeRequest.Note, Is.EqualTo(commentMessage));
        }
    }
}