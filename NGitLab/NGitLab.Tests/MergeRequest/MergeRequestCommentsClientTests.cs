using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.MergeRequest {
    public class MergeRequestCommentsClientTests {
        readonly IMergeRequestCommentClient mergeRequestComments;

        public MergeRequestCommentsClientTests() {
            mergeRequestComments = _MergeRequestClientTests.MergeRequestClient.Comments(5);
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllComments() {
            mergeRequestComments.All().ShouldNotBeEmpty();
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