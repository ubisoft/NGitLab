using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestCommentsClientTests
    {
        private IMergeRequestClient _mergeRequestClient;
        private Project _project;

        private Models.MergeRequest _mergeRequest;

        private Models.MergeRequest MergeRequest
        {
            get
            {
                if (_mergeRequest == null)
                {
                    _mergeRequest = _mergeRequestClient.Create(new MergeRequestCreate
                    {
                        Title = "Test merge request comments",
                        SourceBranch = "master",
                        TargetBranch = "newbranch"
                    });
                }

                return _mergeRequest;
            }
        }

        [SetUp]
        public void Setup()
        {
            _project = Initialize.UnitTestProject;
            _mergeRequestClient = Initialize.GitLabClient.GetMergeRequest(_project.Id);
        }

        [Test]
        [Order(1)]
        public void AddCommentToMergeRequest()
        {
            var mergeRequestComments = _mergeRequestClient.Comments(MergeRequest.Id);
            const string commentMessage = "Comment for MR";
            var newComment = new MergeRequestComment
            {
                Note = commentMessage,
            };
            var comment = mergeRequestComments.Add(newComment);
            Assert.That(comment.Note, Is.EqualTo(commentMessage));
        }

        [Test]
        [Order(2)]
        public void GetAllComments()
        {
            var mergeRequestComments = _mergeRequestClient.Comments(MergeRequest.Id);
            var comments = mergeRequestComments.All.ToArray();
            CollectionAssert.IsNotEmpty(comments);
        }
    }
}