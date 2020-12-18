using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MergeRequestDiscussionClient : ClientBase, IMergeRequestDiscussionClient
    {
        private readonly int _projectId;
        private readonly int _mergeRequestIid;

        public MergeRequestDiscussionClient(ClientContext context, int projectId, int mergeRequestIid)
            : base(context)
        {
            _projectId = projectId;
            _mergeRequestIid = mergeRequestIid;
        }

        private MergeRequest GetMergeRequest() => GetMergeRequest(_projectId, _mergeRequestIid);

        public IEnumerable<Models.MergeRequestDiscussion> All => GetMergeRequest().GetDiscussions();

        public Models.MergeRequestDiscussion Add(Models.MergeRequestComment comment)
        {
            return Add(new MergeRequestDiscussionCreate
            {
                Body = comment.Body,
                CreatedAt = null,
            });
        }

        public Models.MergeRequestDiscussion Add(MergeRequestDiscussionCreate commentCreate)
        {
            EnsureUserIsAuthenticated();

            var project = GetProject(_projectId, ProjectPermission.View);
            if (project.Archived)
                throw new GitLabForbiddenException();

            var comment = new MergeRequestComment
            {
                Author = Context.User,
                Body = commentCreate.Body,
            };

            GetMergeRequest().Comments.Add(comment);

            return new MergeRequestDiscussion()
            {
                Id = comment.ThreadId,
                IndividualNote = false,
                Notes = new[] { comment.ToMergeRequestCommentClient() },
            };
        }

        public MergeRequestDiscussion Resolve(MergeRequestDiscussionResolve resolve)
        {
            var discussions = GetMergeRequest().GetDiscussions();
            var discussion = discussions.First(x => string.Equals(x.Id, resolve.Id, System.StringComparison.Ordinal));
            if (discussion == null)
                throw new GitLabNotFoundException();

            foreach (var note in discussion.Notes)
            {
                note.Resolved = true;
            }

            return discussion;
        }

        public void Delete(string discussionId, long noteId)
        {
            var discussions = GetMergeRequest().GetDiscussions();
            var discussion = discussions.First(x => string.Equals(x.Id, discussionId, System.StringComparison.Ordinal));
            if (discussion == null)
                throw new GitLabNotFoundException();

            var allComments = GetMergeRequest().Comments;
            foreach (var discussionNote in discussion.Notes.Where(x => x.Id == noteId))
            {
                var note = allComments.FirstOrDefault(x => x.Id == discussionNote.Id);
                if (note != null)
                {
                    allComments.Remove(note);
                }
            }
        }
    }
}
