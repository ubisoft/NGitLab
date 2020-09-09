using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MergeRequestClient : ClientBase, IMergeRequestClient
    {
        private readonly int? _projectId;

        public MergeRequestClient(ClientContext context)
            : base(context)
        {
        }

        public MergeRequestClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        private void AssertProjectId()
        {
            if (_projectId == null)
                throw new InvalidOperationException("Valid only for a specific project");

        }

        public Models.MergeRequest this[int iid]
        {
            get
            {
                AssertProjectId();

                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
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
                if (_projectId == null)
                {
                    return Server.AllProjects
                        .Where(project => project.CanUserViewProject(Context.User))
                        .SelectMany(project => project.MergeRequests)
                        .Select(mr => mr.ToMergeRequestClient());
                }

                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                return project.MergeRequests.Select(mr => mr.ToMergeRequestClient());
            }
        }

        public Models.MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message)
        {
            AssertProjectId();
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

            if (project.ApprovalsBeforeMerge > 0 && !mergeRequest.Approvers.Any())
            {
                throw new GitLabException("The merge request needs to be approved before merging")
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };
            }

            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            if (message.Sha != null)
            {
                var commit = project.Repository.GetBranchTipCommit(mergeRequest.SourceBranch);
                if (!string.Equals(commit.Sha, message.Sha, StringComparison.OrdinalIgnoreCase))
                {
                    throw new GitLabException("SHA does not match HEAD of source branch")
                    {
                        StatusCode = HttpStatusCode.Conflict,
                    };
                }
            }

            if (project.MergeMethod != null &&
                (string.Equals(project.MergeMethod, "ff", StringComparison.Ordinal) || string.Equals(project.MergeMethod, "rebase_merge", StringComparison.Ordinal)) &&
                project.Repository.IsRebaseNeeded(mergeRequest.SourceBranch, mergeRequest.TargetBranch))
            {
                throw new GitLabException("The merge request has some conflicts and cannot be merged")
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                };
            }

            mergeRequest.Accept(Context.User);
            return mergeRequest.ToMergeRequestClient();
        }

        public Models.MergeRequest Accept(int mergeRequestIid, MergeRequestMerge message)
        {
            AssertProjectId();
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

            if (project.ApprovalsBeforeMerge > 0 && !mergeRequest.Approvers.Any())
            {
                throw new GitLabException("The merge request needs to be approved before merging")
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };
            }

            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            if (message.Sha != null)
            {
                var commit = project.Repository.GetBranchTipCommit(mergeRequest.SourceBranch);
                if (!string.Equals(commit.Sha, message.Sha, StringComparison.OrdinalIgnoreCase))
                {
                    throw new GitLabException("SHA does not match HEAD of source branch")
                    {
                        StatusCode = HttpStatusCode.Conflict,
                    };
                }
            }

            if (project.MergeMethod != null &&
                (string.Equals(project.MergeMethod, "ff", StringComparison.Ordinal) || string.Equals(project.MergeMethod, "rebase_merge", StringComparison.Ordinal)) &&
                project.Repository.IsRebaseNeeded(mergeRequest.SourceBranch, mergeRequest.TargetBranch))
            {
                throw new GitLabException("The merge request has some conflicts and cannot be merged")
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                };
            }

            mergeRequest.Accept(Context.User);
            return mergeRequest.ToMergeRequestClient();
        }

        public Models.MergeRequest Approve(int mergeRequestIid, MergeRequestApprove message)
        {
            AssertProjectId();
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            project.CanUserContributeToProject(Context.User);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            // Check if user has already aproved the merge request
            if (mergeRequest.Approvers.Where(x => x.Id == Context.User.Id).Any())
            {
                throw new GitLabException("GitLab server returned an error (Unauthorized): Empty Response.")
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };
            }

            /* To be implemented 
             * need get configuration for gitlab merge request approval: https://docs.gitlab.com/ee/api/merge_request_approvals.html)
             * 1) Check if project approval rules require password input
             * 2) Check if project approval rules prevents merge request committers from approving
             * 3) Check if project approval rules prevents merge request author from approving
             */

            if (message.Sha != null)
            {
                var commit = project.Repository.GetBranchTipCommit(mergeRequest.SourceBranch);
                if (!string.Equals(commit.Sha, message.Sha, StringComparison.OrdinalIgnoreCase))
                {
                    throw new GitLabException("SHA does not match HEAD of source branch")
                    {
                        StatusCode = HttpStatusCode.Conflict,
                    };
                }
            }

            mergeRequest.Approvers.Add(new UserRef(Context.User));
            return mergeRequest.ToMergeRequestClient();
        }

        public RebaseResult Rebase(int mergeRequestIid)
        {
            AssertProjectId();
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            return mergeRequest.Rebase(Context.User);
        }

        public IEnumerable<Models.MergeRequest> AllInState(MergeRequestState state)
        {
            if (_projectId == null)
            {
                return Server.AllProjects
                    .Where(project => project.CanUserViewProject(Context.User))
                    .SelectMany(project => project.MergeRequests)
                    .Where(mr => mr.State == state)
                    .Select(mr => mr.ToMergeRequestClient());
            }

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
            return project.MergeRequests.Where(mr => mr.State == state).Select(mr => mr.ToMergeRequestClient());
        }

        public IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid)
        {
            AssertProjectId();

            return new MergeRequestApprovalClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
        }

        public Models.MergeRequest Close(int mergeRequestIid)
        {
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
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
            AssertProjectId();

            return new MergeRequestCommentClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
        }

        public IMergeRequestCommitClient Commits(int mergeRequestIid)
        {
            AssertProjectId();

            return new MergeRequestCommitClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
        }

        public Models.MergeRequest Create(MergeRequestCreate mergeRequestCreate)
        {
            AssertProjectId();

            EnsureUserIsAuthenticated();

            var sourceProject = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.Contribute);
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
            SetLabels(mergeRequest, mergeRequestCreate.Labels);

            return mergeRequest.ToMergeRequestClient();
        }

        private static void SetLabels(MergeRequest mergeRequest, string labels)
        {
            if (labels != null)
            {
                mergeRequest.Labels.Clear();
                foreach (var label in labels.Split(','))
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        mergeRequest.Labels.Add(label);
                    }
                }
            }
        }

        public void Delete(int mergeRequestIid)
        {
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            project.MergeRequests.Remove(mergeRequest);
        }

        public IEnumerable<Models.MergeRequest> Get(MergeRequestQuery query)
        {
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
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
                    result = result.Where(mr => string.Equals(mr.SourceBranch, query.SourceBranch, StringComparison.Ordinal));
                }

                if (query.TargetBranch != null)
                {
                    result = result.Where(mr => string.Equals(mr.TargetBranch, query.TargetBranch, StringComparison.Ordinal));
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

                if (query.Wip != null)
                {
                    result = result.Where(mr => (bool)query.Wip ? mr.WorkInProgress : !mr.WorkInProgress);
                }
            }

            return result.Select(mr => mr.ToMergeRequestClient());
        }

        public IEnumerable<Models.Author> GetParticipants(int mergeRequestIid)
        {
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            return mergeRequest.Comments.Select(c => c.Author)
                .Union(new[] { mergeRequest.Author })
                .Select(u => u.ToClientAuthor());
        }

        public IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid)
        {
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            var latestCommit = mergeRequest.Commits.FirstOrDefault();
            var sha = latestCommit == null ? mergeRequest.Sha : new Sha1(latestCommit.Sha);

            return new[]
            {
                new PipelineBasic
                {
                    Id = 42,
                    Status = JobStatus.Running,
                    Sha = sha,
                    Ref = mergeRequest.TargetBranch,
                },
            };
        }

        public Models.MergeRequest Reopen(int mergeRequestIid)
        {
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
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
            AssertProjectId();

            var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            if (mergeRequestUpdate.AssigneeId != null)
            {
                if (mergeRequestUpdate.AssigneeId.Value == 0)
                {
                    mergeRequest.Assignee = null;
                }
                else
                {
                    var user = Server.Users.GetById(mergeRequestUpdate.AssigneeId.Value);
                    if (user == null)
                        throw new GitLabBadRequestException("user not found");

                    mergeRequest.Assignee = new UserRef(user);
                }
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

            SetLabels(mergeRequest, mergeRequestUpdate.Labels);

            mergeRequest.UpdatedAt = DateTimeOffset.UtcNow;
            return mergeRequest.ToMergeRequestClient();
        }
    }
}
