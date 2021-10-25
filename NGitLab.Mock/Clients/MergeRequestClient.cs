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

                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                    var mergeRequest = project.MergeRequests.GetByIid(iid);
                    if (mergeRequest == null)
                        throw new GitLabNotFoundException();

                    return mergeRequest.ToMergeRequestClient();
                }
            }
        }

        public IEnumerable<Models.MergeRequest> All
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    if (_projectId == null)
                    {
                        return Server.AllProjects
                            .Where(project => project.CanUserViewProject(Context.User))
                            .SelectMany(project => project.MergeRequests)
                            .Select(mr => mr.ToMergeRequestClient())
                            .ToList();
                    }

                    var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                    return project.MergeRequests.Select(mr => mr.ToMergeRequestClient()).ToList();
                }
            }
        }

        public Models.MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message)
        {
            AssertProjectId();
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

                if (project.ApprovalsBeforeMerge > mergeRequest.Approvers.Count)
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
        }

        public Models.MergeRequest Accept(int mergeRequestIid, MergeRequestMerge message)
        {
            AssertProjectId();
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

                if (project.ApprovalsBeforeMerge > mergeRequest.Approvers.Count)
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
        }

        public Models.MergeRequest Approve(int mergeRequestIid, MergeRequestApprove message)
        {
            AssertProjectId();

            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                project.CanUserContributeToProject(Context.User);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                // Check if user has already aproved the merge request
                if (mergeRequest.Approvers.Any(x => x.Id == Context.User.Id))
                {
                    throw new GitLabException("GitLab server returned an error (Unauthorized): Empty Response.")
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                    };
                }

                /* To be implemented
                 * need get configuration for GitLab merge request approval: https://docs.gitlab.com/ee/api/merge_request_approvals.html)
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
        }

        public RebaseResult Rebase(int mergeRequestIid)
        {
            AssertProjectId();
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                return mergeRequest.Rebase(Context.User);
            }
        }

        public IEnumerable<Models.MergeRequest> AllInState(MergeRequestState state)
        {
            using (Context.BeginOperationScope())
            {
                if (_projectId == null)
                {
                    return Server.AllProjects
                        .Where(project => project.CanUserViewProject(Context.User))
                        .SelectMany(project => project.MergeRequests)
                        .Where(mr => mr.State == state)
                        .Select(mr => mr.ToMergeRequestClient())
                        .ToList();
                }

                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                return project.MergeRequests.Where(mr => mr.State == state).Select(mr => mr.ToMergeRequestClient()).ToList();
            }
        }

        public IMergeRequestChangeClient Changes(int mergeRequestIid)
        {
            AssertProjectId();

            return new MergeRequestChangeClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
        }

        public IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid)
        {
            AssertProjectId();

            return new MergeRequestApprovalClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
        }

        public Models.MergeRequest Close(int mergeRequestIid)
        {
            AssertProjectId();

            using (Context.BeginOperationScope())
            {
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

            using (Context.BeginOperationScope())
            {
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

            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                project.MergeRequests.Remove(mergeRequest);
            }
        }

        public IEnumerable<Models.MergeRequest> Get(MergeRequestQuery query)
        {
            using (Context.BeginOperationScope())
            {
                var projects = _projectId == null
                    ? Server.AllProjects.Where(project => project.CanUserViewProject(Context.User))
                    : new[] { GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View) };
                var mergeRequests = projects.SelectMany(x => x.MergeRequests);
                return FilterByQuery(mergeRequests, query);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "There are lots of cases to support")]
        public IEnumerable<Models.MergeRequest> FilterByQuery(IEnumerable<MergeRequest> mergeRequests, MergeRequestQuery query)
        {
            if (query != null)
            {
                if (query.ApproverIds != null)
                {
                    var approverIds = query.ApproverIds;
                    mergeRequests = mergeRequests.Where(mr => mr.Approvers.Any(x => approverIds.Contains(x.Id)));
                }

                if (query.AssigneeId != null)
                {
                    var assigneeId = string.Equals(query.AssigneeId.ToString(), "None", StringComparison.OrdinalIgnoreCase) ? (int?)null : int.Parse(query.AssigneeId.ToString());
                    mergeRequests = mergeRequests.Where(mr => mr.Assignee?.Id == assigneeId);
                }

                if (query.ReviewerId != null)
                {
                    if (query.ReviewerId == QueryAssigneeId.None)
                    {
                        mergeRequests = mergeRequests.Where(mr => mr.Reviewers == null || mr.Reviewers.Count == 0);
                    }
                    else if (query.ReviewerId == QueryAssigneeId.Any)
                    {
                        mergeRequests = mergeRequests.Where(mr => mr.Reviewers != null || mr.Reviewers.Any());
                    }
                    else
                    {
                        var reviewerId = int.Parse(query.ReviewerId.ToString());
                        mergeRequests = mergeRequests.Where(mr => mr.Reviewers.Any(x => reviewerId.Equals(x.Id)));
                    }
                }

                if (query.AuthorId != null)
                {
                    mergeRequests = mergeRequests.Where(mr => mr.Author.Id == query.AuthorId);
                }

                if (query.CreatedAfter != null)
                {
                    mergeRequests = mergeRequests.Where(mr => mr.CreatedAt >= query.CreatedAfter.Value);
                }

                if (query.CreatedBefore != null)
                {
                    mergeRequests = mergeRequests.Where(mr => mr.CreatedAt <= query.CreatedBefore.Value);
                }

                if (!string.IsNullOrEmpty(query.Labels))
                {
                    foreach (var label in query.Labels.Split(','))
                    {
                        mergeRequests = mergeRequests.Where(mr => mr.Labels.Contains(label, StringComparer.Ordinal));
                    }
                }

                if (query.Milestone != null)
                {
                    throw new NotImplementedException();
                }

                if (query.Scope != null)
                {
                    var userId = Context.User.Id;
                    switch (query.Scope)
                    {
                        case "created_by_me":
                        case "created-by-me":
                            mergeRequests = mergeRequests.Where(mr => mr.Author.Id == userId);
                            break;
                        case "assigned_to_me":
                        case "assigned-to-me":
                            mergeRequests = mergeRequests.Where(mr => mr.Assignee?.Id == userId);
                            break;
                        case "all":
                            break;
                        default:
                            throw new NotSupportedException($"Scope '{query.Scope}' is not supported");
                    }
                }

                if (query.Search != null)
                {
                    throw new NotImplementedException();
                }

                if (query.SourceBranch != null)
                {
                    mergeRequests = mergeRequests.Where(mr => string.Equals(mr.SourceBranch, query.SourceBranch, StringComparison.Ordinal));
                }

                if (query.TargetBranch != null)
                {
                    mergeRequests = mergeRequests.Where(mr => string.Equals(mr.TargetBranch, query.TargetBranch, StringComparison.Ordinal));
                }

                if (query.UpdatedAfter != null)
                {
                    mergeRequests = mergeRequests.Where(mr => mr.UpdatedAt >= query.UpdatedAfter.Value);
                }

                if (query.UpdatedBefore != null)
                {
                    mergeRequests = mergeRequests.Where(mr => mr.UpdatedAt <= query.UpdatedBefore);
                }

                if (query.State != null)
                {
                    mergeRequests = mergeRequests.Where(mr => mr.State == query.State);
                }

                if (string.Equals(query.Sort, "asc", StringComparison.Ordinal))
                {
                    mergeRequests = mergeRequests.Reverse();
                }

                if (query.OrderBy != null)
                {
                    mergeRequests = query.OrderBy switch
                    {
                        "created_at" => mergeRequests.OrderBy(mr => mr.CreatedAt),
                        "updated_at" => mergeRequests.OrderBy(mr => mr.UpdatedAt),
                        _ => throw new NotSupportedException($"OrderBy '{query.OrderBy}' is not supported"),
                    };
                }

                if (query.PerPage != null)
                {
                    mergeRequests = mergeRequests.Take(query.PerPage.Value);
                }

                if (query.Wip != null)
                {
                    mergeRequests = mergeRequests.Where(mr => (bool)query.Wip ? mr.WorkInProgress : !mr.WorkInProgress);
                }
            }

            return mergeRequests.Select(mr => mr.ToMergeRequestClient()).ToList();
        }

        public IEnumerable<Models.Author> GetParticipants(int mergeRequestIid)
        {
            AssertProjectId();

            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                return mergeRequest.Comments.Select(c => c.Author)
                    .Union(new[] { mergeRequest.Author })
                    .Select(u => u.ToClientAuthor())
                    .ToList();
            }
        }

        public IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid)
        {
            AssertProjectId();

            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);

                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                var allSha1 = mergeRequest.Commits.Select(m => new Sha1(m.Sha));

                return mergeRequest.SourceProject.Pipelines
                        .Where(p => allSha1.Contains(p.Sha))
                        .OrderByDescending(p => p.CreatedAt)
                        .Select(p => p.ToPipelineBasicClient())
                        .ToList();
            }
        }

        public Models.MergeRequest Reopen(int mergeRequestIid)
        {
            AssertProjectId();

            using (Context.BeginOperationScope())
            {
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
        }

        public Models.MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequestUpdate)
        {
            AssertProjectId();

            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.View);
                var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
                if (mergeRequest == null)
                    throw new GitLabNotFoundException();

                if (mergeRequestUpdate.AssigneeIds != null)
                {
                    mergeRequest.Assignees.Clear();
                    if (mergeRequestUpdate.AssigneeIds.Length == 0)
                    {
                        mergeRequest.Assignee = null;
                    }
                    else
                    {
                        foreach (var assigneeId in mergeRequestUpdate.AssigneeIds)
                        {
                            var user = Server.Users.GetById(mergeRequestUpdate.AssigneeId.Value);
                            if (user == null)
                                throw new GitLabBadRequestException("user not found");

                            mergeRequest.Assignees.Add(new UserRef(user));
                        }

                        mergeRequest.Assignee = mergeRequest.Assignees.First();
                    }
                }
                else if (mergeRequestUpdate.AssigneeId != null)
                {
                    mergeRequest.Assignees.Clear();
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
                        mergeRequest.Assignees.Add(new UserRef(user));
                    }
                }

                if (mergeRequestUpdate.ReviewerIds != null)
                {
                    foreach (var userId in mergeRequestUpdate.ReviewerIds)
                    {
                        var reviewer = Server.Users.GetById(userId);
                        if (reviewer == null)
                            throw new GitLabBadRequestException("user not found");

                        mergeRequest.Reviewers.Add(reviewer);
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

        public IEnumerable<Models.Issue> ClosesIssues(int mergeRequestIid)
        {
            throw new NotImplementedException();
        }

        public IMergeRequestDiscussionClient Discussions(int mergeRequestIid)
        {
            AssertProjectId();

            return new MergeRequestDiscussionClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
        }
    }
}
