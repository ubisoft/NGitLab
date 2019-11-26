using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MergeRequestClient : ClientBase, IMergeRequestClient
    {
        private readonly int _projectId;

        public MergeRequestClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public Models.MergeRequest this[int iid]
        {
            get
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var mergeRequest = project.MergeRequests.GetByIid(iid);
                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                return mergeRequest.ToMergeRequestClient();
            }
        }

        public IEnumerable<Models.MergeRequest> All
        {
            get
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.MergeRequests.Select(mr => mr.ToMergeRequestClient());
            }
        }

        public Models.MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            mergeRequest.Accept(Context.User);
            return mergeRequest.ToMergeRequestClient();
        }

        public IEnumerable<Models.MergeRequest> AllInState(MergeRequestState state)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            return project.MergeRequests.Where(mr => mr.State == state).Select(mr => mr.ToMergeRequestClient());
        }

        public IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid)
        {
            return new MergeRequestApprovalClient(Context, _projectId, mergeRequestIid);
        }

        public Models.MergeRequest Close(int mergeRequestIid)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            if (mergeRequest.State != MergeRequestState.opened)
                throw new GitLabBadRequestException();

            mergeRequest.ClosedAt = DateTimeOffset.UtcNow;
            mergeRequest.UpdatedAt = DateTimeOffset.UtcNow;
            return mergeRequest.ToMergeRequestClient();
        }

        public IMergeRequestCommentClient Comments(int mergeRequestIid)
        {
            return new MergeRequestCommentClient(Context, _projectId, mergeRequestIid);
        }

        public IMergeRequestCommitClient Commits(int mergeRequestIid)
        {
            return new MergeRequestCommitClient(Context, _projectId, mergeRequestIid);
        }

        public Models.MergeRequest Create(MergeRequestCreate mergeRequestCreate)
        {
            EnsureUserIsAuthenticated();

            var sourceProject = GetProject(_projectId, ProjectPermission.Contribute);
            var targetProject = GetProject(mergeRequestCreate.TargetProjectId, ProjectPermission.View);

            // Ensure the branches exist
            _ = sourceProject.Repository.GetBranch(mergeRequestCreate.SourceBranch) ?? throw new GitLabBadRequestException("Source branch not found");
            _ = targetProject.Repository.GetBranch(mergeRequestCreate.TargetBranch) ?? throw new GitLabBadRequestException("Target branch not found");

            UserRef assignee = null;
            if (mergeRequestCreate.AssigneeId != null)
            {
                assignee = Server.Users.GetById(mergeRequestCreate.AssigneeId.Value) ?? throw new GitLabBadRequestException("assignee not found");
            }

            var mergeRequest = targetProject.MergeRequests.Add(sourceProject, mergeRequestCreate.SourceBranch, mergeRequestCreate.TargetBranch, mergeRequestCreate.Title, Context.User);
            mergeRequest.Assignee = assignee;
            mergeRequest.Description = mergeRequestCreate.Description;
            mergeRequest.ShouldRemoveSourceBranch = mergeRequestCreate.RemoveSourceBranch;
            mergeRequest.Squash = mergeRequestCreate.Squash;
            return mergeRequest.ToMergeRequestClient();
        }

        public void Delete(int mergeRequestIid)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            project.MergeRequests.Remove(mergeRequest);
        }

        public IEnumerable<Models.MergeRequest> Get(MergeRequestQuery query)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            IEnumerable<MergeRequest> result = project.MergeRequests;
            if (query != null)
            {
                if (query.ApproverIds != null)
                {
                    throw new NotImplementedException();
                }

                if (query.AssigneeId != null)
                {
                    throw new NotImplementedException();
                }

                if (query.AuthorId != null)
                {
                    result = result.Where(mr => mr.Author.Id == query.AuthorId);
                }

                if (query.CreatedAfter != null)
                {
                    result = result.Where(mr => mr.CreatedAt >= query.CreatedAfter.Value);
                }

                if (query.CreatedBefore != null)
                {
                    result = result.Where(mr => mr.CreatedAt <= query.CreatedBefore.Value);
                }

                if (query.Labels != null)
                {
                    throw new NotImplementedException();
                }

                if (query.Milestone != null)
                {
                    throw new NotImplementedException();
                }

                if (query.Scope != null)
                {
                    throw new NotImplementedException();
                }

                if (query.Search != null)
                {
                    throw new NotImplementedException();
                }

                if (query.SourceBranch != null)
                {
                    result = result.Where(mr => mr.SourceBranch == query.SourceBranch);
                }

                if (query.TargetBranch != null)
                {
                    result = result.Where(mr => mr.TargetBranch == query.TargetBranch);
                }

                if (query.UpdatedAfter != null)
                {
                    result = result.Where(mr => mr.UpdatedAt >= query.UpdatedAfter.Value);
                }

                if (query.UpdatedBefore != null)
                {
                    result = result.Where(mr => mr.UpdatedAt <= query.UpdatedBefore);
                }

                if (query.State != null)
                {
                    result = result.Where(mr => mr.State == query.State);
                }

                if (query.Sort != null)
                {
                    throw new NotImplementedException();
                }

                if (query.OrderBy != null)
                {
                    throw new NotImplementedException();
                }

                if (query.PerPage != null)
                {
                    result = result.Take(query.PerPage.Value);
                }
            }

            return result.Select(mr => mr.ToMergeRequestClient());
        }

        public IEnumerable<Models.Author> GetParticipants(int mergeRequestIid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid)
        {
            throw new NotImplementedException();
        }

        public Models.MergeRequest Reopen(int mergeRequestIid)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            if (mergeRequest.State != MergeRequestState.closed)
                throw new GitLabBadRequestException();

            mergeRequest.ClosedAt = null;
            mergeRequest.UpdatedAt = DateTimeOffset.UtcNow;
            return mergeRequest.ToMergeRequestClient();
        }

        public Models.MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequestUpdate)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            if (mergeRequestUpdate.AssigneeId != null)
            {
                var user = Server.Users.GetById(mergeRequestUpdate.AssigneeId.Value);
                if (user == null)
                    throw new GitLabBadRequestException("user not found");

                mergeRequest.Assignee = new UserRef(user);
            }

            if (mergeRequestUpdate.Description != null)
            {
                mergeRequest.Description = mergeRequestUpdate.Description;
            }

            if (mergeRequestUpdate.NewState != null)
            {
                throw new NotImplementedException();
            }

            if (mergeRequestUpdate.SourceBranch != null)
            {
                mergeRequest.SourceBranch = mergeRequestUpdate.SourceBranch;
            }

            if (mergeRequestUpdate.TargetBranch != null)
            {
                mergeRequest.TargetBranch = mergeRequestUpdate.TargetBranch;
            }

            if (mergeRequestUpdate.Title != null)
            {
                mergeRequest.Title = mergeRequestUpdate.Title;
            }

            mergeRequest.UpdatedAt = DateTimeOffset.UtcNow;
            return mergeRequest.ToMergeRequestClient();
        }
    }
}
