using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestCommentsClientTests
    {
        private IMergeRequestCommentClient _mergeRequestComments;
        public static IMergeRequestClient MergeRequestClient;
        public static Project Project;

        [SetUp]
        public void Setup()
        {
            Project = Initialize.GitLabClient.Projects.Owned.First(project => project.Name == "Diaspora Client");
            MergeRequestClient = Initialize.GitLabClient.GetMergeRequest(Project.Id);
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void GetAllComments()
        {
            _mergeRequestComments = MergeRequestClient.Comments(5);
            var comments = _mergeRequestComments.All.ToArray();
            CollectionAssert.IsNotEmpty(comments);
        }

        [Test]
        [Ignore("GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!")]
        public void AddCommentToMergeRequest()
        {
            _mergeRequestComments = MergeRequestClient.Comments(4);
            const string commentMessage = "Comment for MR";
            var newComment = new MergeRequestComment {Body = commentMessage};
            var comment = _mergeRequestComments.Add(newComment);
            Assert.That(comment.Body, Is.EqualTo(commentMessage));
        }
    }
}