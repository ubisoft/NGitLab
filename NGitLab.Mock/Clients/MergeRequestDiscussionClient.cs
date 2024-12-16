using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MergeRequestDiscussionClient : ClientBase, IMergeRequestDiscussionClient
{
    private readonly long _projectId;
    private readonly long _mergeRequestIid;

    public MergeRequestDiscussionClient(ClientContext context, long projectId, long mergeRequestIid)
        : base(context)
    {
        _projectId = projectId;
        _mergeRequestIid = mergeRequestIid;
    }

    private MergeRequest GetMergeRequest() => GetMergeRequest(_projectId, _mergeRequestIid);

    public IEnumerable<MergeRequestDiscussion> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return GetMergeRequest().GetDiscussions().ToList();
            }
        }
    }

    public MergeRequestDiscussion Get(string id)
    {
        using (Context.BeginOperationScope())
        {
            var discussions = GetMergeRequest().GetDiscussions();
            var discussion = discussions.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.Ordinal));
            return discussion ?? throw new GitLabNotFoundException();
        }
    }

    public Task<MergeRequestDiscussion> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Get(id));
    }

    public MergeRequestDiscussion Add(Models.MergeRequestComment comment)
    {
        return Add(new MergeRequestDiscussionCreate
        {
            Body = comment.Body,
            CreatedAt = null,
        });
    }

    public MergeRequestDiscussion Add(MergeRequestDiscussionCreate commentCreate)
    {
        EnsureUserIsAuthenticated();

        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            if (project.Archived)
                throw new GitLabForbiddenException();

            var comment = new MergeRequestComment
            {
                Author = Context.User,
                Body = commentCreate.Body,
            };

            GetMergeRequest().Comments.Add(comment);

            return new MergeRequestDiscussion
            {
                Id = comment.ThreadId,
                IndividualNote = false,
                Notes = new[] { comment.ToMergeRequestCommentClient() },
            };
        }
    }

    public MergeRequestDiscussion Resolve(MergeRequestDiscussionResolve resolve)
    {
        using (Context.BeginOperationScope())
        {
            var discussions = GetMergeRequest().GetDiscussions();
            var discussion = discussions.FirstOrDefault(x => string.Equals(x.Id, resolve.Id, StringComparison.Ordinal));
            if (discussion == null)
                throw new GitLabNotFoundException();

            foreach (var note in discussion.Notes)
            {
                note.Resolved = true;
            }

            return discussion;
        }
    }

    public void Delete(string discussionId, long noteId)
    {
        using (Context.BeginOperationScope())
        {
            var discussions = GetMergeRequest().GetDiscussions();
            var discussion = discussions.FirstOrDefault(x => string.Equals(x.Id, discussionId, StringComparison.Ordinal));
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
