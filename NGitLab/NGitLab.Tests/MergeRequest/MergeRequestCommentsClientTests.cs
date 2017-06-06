using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestCommentsClientTests
    {
        private readonly IMergeRequestCommentClient _mergeRequestComments;

        public MergeRequestCommentsClientTests()
        {
            _mergeRequestComments = _MergeRequestClientTests.MergeRequestClient.Comments(5);
        }

        [Test]
        [Category("Server_Required")]
        public void GetAllComments()
        {
            _mergeRequestComments.All().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void AddCommentToMergeRequest()
        {
            const string commentMessage = "note";
            var newComment = new MergeRequestComment {Note = commentMessage};

            var mergeRequest = _mergeRequestComments.Add(newComment);

            Assert.That(mergeRequest.Note, Is.EqualTo(commentMessage));
        }
    }
}