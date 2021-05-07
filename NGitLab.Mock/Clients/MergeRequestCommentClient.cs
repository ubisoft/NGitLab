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

        public IEnumerable<Models.MergeRequestComment> All
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return GetMergeRequest().Comments.Select(mr => mr.ToMergeRequestCommentClient()).ToList();
                }
            }
        }

        public IEnumerable<MergeRequestDiscussion> Discussions
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return GetMergeRequest().GetDiscussions().ToList();
                }
            }
        }

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
                return comment.ToMergeRequestCommentClient();
            }
        }

        public Models.MergeRequestComment Edit(long id, MergeRequestCommentEdit edit)
        {
            using (Context.BeginOperationScope())
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
        }

        public void Delete(long id)
        {
            using (Context.BeginOperationScope())
            {
                var comments = GetMergeRequest().Comments;
                var comment = comments.GetById(id);
                if (comment == null)
                    throw new GitLabNotFoundException();

                comments.Remove(comment);
            }
        }

        public IEnumerable<Models.MergeRequestComment> Get(MergeRequestCommentQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
