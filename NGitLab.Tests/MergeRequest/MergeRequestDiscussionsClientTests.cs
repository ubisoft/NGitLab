using System;
using System.Linq;
using System.Net;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest
{
    public class MergeRequestDiscussionsClientTests
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
                Name = "mr-discussions-test",
                Ref = "master",
            });

            Initialize.Repository.Files.Create(new FileUpsert
            {
                RawContent = "test content",
                CommitMessage = "commit to merge",
                Branch = branch.Name,
                Path = "mr-discussions-test.md",
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
        public void AddDiscussionToMergeRequest_DiscussionCreated()
        {
            var mergeRequestDiscussions = _mergeRequestClient.Discussions(MergeRequest.Iid);
            const string discussionMessage = "Discussion for MR";
            var newDiscussion = new MergeRequestDiscussionCreate
            {
                Body = discussionMessage,
            };
            var discussion = mergeRequestDiscussions.Add(newDiscussion);
            Assert.That(discussion.IndividualNote, Is.False);
            Assert.That(discussion.Notes[0].Body, Is.EqualTo(discussionMessage));
            Assert.That(discussion.Notes[0].Resolvable, Is.True);
            Assert.That(discussion.Notes[0].Resolved, Is.False);

            var discussions = mergeRequestDiscussions.All.ToArray();
            CollectionAssert.IsNotEmpty(discussions);
        }

        [Test]
        public void EditCommentFromDiscussion_CommentEdited()
        {
            var mergeRequestDiscussions = _mergeRequestClient.Discussions(MergeRequest.Iid);

            // Create a discussion, it creates automatically one note
            const string discussionMessage = "Discussion for MR";
            var createdAt = new DateTime(2019, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var newDiscussion = new MergeRequestDiscussionCreate
            {
                Body = discussionMessage,
                CreatedAt = createdAt,
            };
            var discussion = mergeRequestDiscussions.Add(newDiscussion);
            Assert.That(discussion.Notes[0].Body, Is.EqualTo(discussionMessage));
            Assert.That(discussion.Notes[0].CreatedAt, Is.EqualTo(createdAt));

            // Edit the note associated with the discussion
            const string discussionMessageEdit = "Discussion for MR edit";
            var mergeRequestComments = _mergeRequestClient.Comments(MergeRequest.Iid);

            var editedComment = mergeRequestComments.Edit(discussion.Notes[0].Id, new MergeRequestCommentEdit { Body = discussionMessageEdit });
            Assert.That(editedComment.Id, Is.EqualTo(discussion.Notes[0].Id));
            Assert.That(editedComment.Body, Is.EqualTo(discussionMessageEdit));
            Assert.That(editedComment.CreatedAt, Is.EqualTo(createdAt));
            Assert.That(editedComment.Resolved, Is.False);
        }

        [Test]
        public void AddDiscussionToMergeRequestOnArchivedProject()
        {
            var mergeRequestDiscussions = _mergeRequestClient.Discussions(MergeRequest.Iid);
            const string discussionMessage = "Discussion for MR";
            var newDiscussion = new MergeRequestDiscussionCreate
            {
                Body = discussionMessage,
            };

            var projectClient = Initialize.GitLabClient.Projects;
            projectClient.Archive(_project.Id);

            try
            {
                var ex = Assert.Throws<GitLabException>(() => mergeRequestDiscussions.Add(newDiscussion));
                Assert.AreEqual(ex.StatusCode, HttpStatusCode.Forbidden);
            }
            finally
            {
                projectClient.Unarchive(_project.Id);
            }
        }

        [Test]
        public void ResolveDiscussion_AllNotesResolved()
        {
            var mergeRequestDiscussions = _mergeRequestClient.Discussions(MergeRequest.Iid);

            const string discussionMessage = "Discussion for MR";
            var newDiscussion = new MergeRequestDiscussionCreate
            {
                Body = discussionMessage,
            };
            var discussion = mergeRequestDiscussions.Add(newDiscussion);

            var resolve = new MergeRequestDiscussionResolve()
            {
                Id = discussion.Id,
                Resolved = true,
            };
            var resolvedDiscussion = mergeRequestDiscussions.Resolve(resolve);

            Assert.That(resolvedDiscussion.Notes[0].Resolved, Is.True);
        }

        [Test]
        public void DeleteOneNoteFromDiscussion_DiscussionAndNoteDeleted()
        {
            var mergeRequestDiscussions = _mergeRequestClient.Discussions(MergeRequest.Iid);

            const string discussionMessage = "Discussion for MR";
            var newDiscussion = new MergeRequestDiscussionCreate
            {
                Body = discussionMessage,
            };
            var discussion = mergeRequestDiscussions.Add(newDiscussion);
            mergeRequestDiscussions.Delete(discussion.Id, discussion.Notes[0].Id);

            var discussions = mergeRequestDiscussions.All.ToArray();
            Assert.That(discussions.All(x => !string.Equals(x.Id, discussion.Id, StringComparison.Ordinal)), Is.True);
        }
    }
}
