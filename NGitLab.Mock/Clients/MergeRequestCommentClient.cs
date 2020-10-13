using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MergeRequestCommentClient : ClientBase, IMergeRequestCommentClient
    {
        private readonly int _projectId;
        private readonly int _mergeRequestIid;

        public MergeRequestCommentClient(ClientContext context, int projectId, int mergeRequestIid)
            : base(context)
        {
            _projectId = projectId;
            _mergeRequestIid = mergeRequestIid;
        }

        private MergeRequest GetMergeRequest() => GetMergeRequest(_projectId, _mergeRequestIid);

        public IEnumerable<Models.MergeRequestComment> All => GetMergeRequest().Comments.Select(mr => mr.ToMergeRequestCommentClient());

        public IEnumerable<MergeRequestDiscussion> Discussions => GetMergeRequest().GetDiscussions();

        public Models.MergeRequestComment Add(Models.MergeRequestComment comment)
        {
            return Add(new MergeRequestCommentCreate
            {
                Body = comment.Body,
                CreatedAt = null,
            });
        }

        public Models.MergeRequestComment Add(MergeRequestCommentCreate commentCreate)
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
            return comment.ToMergeRequestCommentClient();
        }

        public Models.MergeRequestComment Edit(long id, MergeRequestCommentEdit edit)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            if (project.Archived)
                throw new GitLabForbiddenException();

            var comment = GetMergeRequest().Comments.GetById(id);
            if (comment == null)
                throw new GitLabNotFoundException();

            comment.Body = edit.Body;
            return comment.ToMergeRequestCommentClient();
        }

        public void Delete(long id)
        {
            var comments = GetMergeRequest().Comments;
            var comment = comments.GetById(id);
            if (comment == null)
                throw new GitLabNotFoundException();

            comments.Remove(comment);
        }
    }
}
