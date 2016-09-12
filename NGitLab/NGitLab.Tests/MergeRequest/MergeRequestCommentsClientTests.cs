using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestCommentsClientTests
    {
        private IMergeRequestCommentClient _mergeRequestComments;

        [Test]
        public void GetAllComments()
        {
            _mergeRequestComments = _MergeRequestClientTests.MergeRequestClient.Comments(5);
            var comments = _mergeRequestComments.All.ToArray();
            CollectionAssert.IsNotEmpty(comments);
        }

        [Test]
        public void AddCommentToMergeRequest()
        {
            _mergeRequestComments = _MergeRequestClientTests.MergeRequestClient.Comments(4);
            const string commentMessage = "Comment for MR";
            var newComment = new MergeRequestComment {Body = commentMessage};
            var comment = _mergeRequestComments.Add(newComment);
            Assert.That(comment.Body, Is.EqualTo(commentMessage));
        }
    }
}