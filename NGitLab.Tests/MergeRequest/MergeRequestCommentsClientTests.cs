using System;
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
                    var branch = CreateBranch();
                    _mergeRequest = _mergeRequestClient.Create(new MergeRequestCreate
                    {
                        Title = "Test merge request comments",
                        SourceBranch = branch.Name,
                        TargetBranch = "master",
                    });
                }

                return _mergeRequest;
            }
        }

        private static Branch CreateBranch()
        {
            var branch = Initialize.Repository.Branches.Create(new BranchCreate
            {
                Name = "mr-comments-test",
                Ref = "master",
            });

            Initialize.Repository.Files.Create(new FileUpsert
            {
                RawContent = "test content",
                CommitMessage = "commit to merge",
                Branch = branch.Name,
                Path = "mr-comments-test.md",
            });

            return branch;
        }

        [SetUp]
        public void Setup()
        {
            _project = Initialize.UnitTestProject;
            _mergeRequestClient = Initialize.GitLabClient.GetMergeRequest(_project.Id);
        }

        [Test]
        [Order(1)]
        public void AddCommentToMergeRequest_DeprecatedApi()
        {
            var mergeRequestComments = _mergeRequestClient.Comments(MergeRequest.Iid);
            const string commentMessage = "Comment for MR";
            var newComment = new MergeRequestCommentCreate
            {
                Body = commentMessage,
            };
            var comment = mergeRequestComments.Add(newComment);
            Assert.That(comment.Body, Is.EqualTo(commentMessage));
        }

        [Test]
        [Order(1)]
        public void AddEditCommentToMergeRequest()
        {
            var mergeRequestComments = _mergeRequestClient.Comments(MergeRequest.Iid);

            // add note
            const string commentMessage = "Comment for MR";
            var createdAt = new DateTime(2019, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var newComment = new MergeRequestCommentCreate
            {
                Body = commentMessage,
                CreatedAt = createdAt,
            };
            var comment = mergeRequestComments.Add(newComment);
            Assert.That(comment.Body, Is.EqualTo(commentMessage));
            Assert.That(comment.CreatedAt, Is.EqualTo(createdAt));

            // edit note
            const string commentMessageEdit = "Comment for MR edit";
            var editedComment = mergeRequestComments.Edit(comment.Id, new MergeRequestCommentEdit { Body = commentMessageEdit });
            Assert.That(editedComment.Id, Is.EqualTo(comment.Id));
            Assert.That(editedComment.Body, Is.EqualTo(commentMessageEdit));
            Assert.That(editedComment.CreatedAt, Is.EqualTo(createdAt));
        }

        [Test]
        [Order(2)]
        public void GetAllComments()
        {
            var mergeRequestComments = _mergeRequestClient.Comments(MergeRequest.Iid);
            var comments = mergeRequestComments.All.ToArray();
            CollectionAssert.IsNotEmpty(comments);
        }
    }
}
