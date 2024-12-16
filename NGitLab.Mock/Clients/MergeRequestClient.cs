using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MergeRequestClient : ClientBase, IMergeRequestClient
{
    private readonly long? _projectId;

    public MergeRequestClient(ClientContext context)
        : base(context)
    {
    }

    public MergeRequestClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public MergeRequestClient(ClientContext context, GroupId groupId)
        : base(context)
    {
        // Support for group-level Merge Requests is not implemented in the mocks yet.
        // For this reason, the GetGroupMergeRequest() method is excluded from the test cases in GitLabClientMockTest (see GroupClientTestCases).
        // The exclusion in the test should be removed when support for support for merge requests in groups is implemented.
        throw new NotImplementedException();
    }

    private void AssertProjectId()
    {
        if (_projectId == null)
            throw new InvalidOperationException("Valid only for a specific project");
    }

    public Models.MergeRequest this[long iid]
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

    public Task<Models.MergeRequest> GetByIidAsync(long iid, SingleMergeRequestQuery options, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(this[iid]);
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

    public Models.MergeRequest Accept(long mergeRequestIid, MergeRequestAccept message)
    {
        return Accept(mergeRequestIid, new MergeRequestMerge
        {
            Sha = message.Sha,
            ShouldRemoveSourceBranch = message.ShouldRemoveSourceBranch,
        });
    }

    public Models.MergeRequest Accept(long mergeRequestIid, MergeRequestMerge message)
    {
        AssertProjectId();
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
            if (mergeRequest == null)
                throw new GitLabNotFoundException();

            mergeRequest.ShouldRemoveSourceBranch = message.ShouldRemoveSourceBranch ?? false;

            if (project.ApprovalsBeforeMerge > mergeRequest.Approvers.Count)
            {
                throw new GitLabException("The merge request needs to be approved before merging")
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };
            }

            if (message.Sha != null)
            {
                var commit = mergeRequest.SourceBranchHeadCommit;
                if (!string.Equals(commit.Sha, message.Sha, StringComparison.OrdinalIgnoreCase))
                {
                    throw new GitLabException("SHA does not match HEAD of source branch")
                    {
                        StatusCode = HttpStatusCode.Conflict,
                    };
                }
            }

            if (mergeRequest.HasConflicts)
            {
                throw new GitLabException("The merge request has some conflicts and cannot be merged")
                {
                    StatusCode = HttpStatusCode.NotAcceptable,
                };
            }

            if (project.MergeMethod != null &&
                (string.Equals(project.MergeMethod, "ff", StringComparison.Ordinal) || string.Equals(project.MergeMethod, "rebase_merge", StringComparison.Ordinal)) &&
                project.Repository.IsRebaseNeeded(mergeRequest.ConsolidatedSourceBranch, mergeRequest.TargetBranch))
            {
                throw new GitLabException($"The MR cannot be merged with method '{project.MergeMethod}': the source branch must first be rebased")
                {
                    StatusCode = HttpStatusCode.MethodNotAllowed,
                };
            }

            mergeRequest.Accept(Context.User);
            return mergeRequest.ToMergeRequestClient();
        }
    }

    public Models.MergeRequest Approve(long mergeRequestIid, MergeRequestApprove message)
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

    public RebaseResult Rebase(long mergeRequestIid)
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

    public Task<RebaseResult> RebaseAsync(long mergeRequestIid, MergeRequestRebase options, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Rebase(mergeRequestIid));
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

            if (!project.AccessibleMergeRequests)
            {
                throw new GitLabException("403 Forbidden")
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }

            return project.MergeRequests.Where(mr => mr.State == state).Select(mr => mr.ToMergeRequestClient()).ToList();
        }
    }

    public IMergeRequestChangeClient Changes(long mergeRequestIid)
    {
        AssertProjectId();

        return new MergeRequestChangeClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
    }

    public IMergeRequestApprovalClient ApprovalClient(long mergeRequestIid)
    {
        AssertProjectId();

        return new MergeRequestApprovalClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
    }

    public Models.MergeRequest Close(long mergeRequestIid)
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

            Server.ResourceStateEvents.CreateResourceStateEvent(Context.User, "closed", mergeRequest.Id, "MergeRequest");
            return mergeRequest.ToMergeRequestClient();
        }
    }

    public IMergeRequestCommentClient Comments(long mergeRequestIid)
    {
        AssertProjectId();

        return new MergeRequestCommentClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
    }

    public IMergeRequestCommitClient Commits(long mergeRequestIid)
    {
        AssertProjectId();

        return new MergeRequestCommitClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
    }

    public Models.MergeRequest CancelMergeWhenPipelineSucceeds(long mergeRequestIid)
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

            mergeRequest.MergeWhenPipelineSucceeds = false;
            mergeRequest.UpdatedAt = DateTimeOffset.UtcNow;
            return mergeRequest.ToMergeRequestClient();
        }
    }

    public Models.MergeRequest Create(MergeRequestCreate mergeRequestCreate)
    {
        AssertProjectId();

        EnsureUserIsAuthenticated();

        using (Context.BeginOperationScope())
        {
            var sourceProject = GetProject(_projectId.GetValueOrDefault(), ProjectPermission.Contribute);
            var targetProject = GetProject(mergeRequestCreate.TargetProjectId ?? _projectId.GetValueOrDefault(), ProjectPermission.View);

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
            SetLabels(mergeRequest, mergeRequestCreate.Labels, labelsToAdd: null, labelsToRemove: null);

            return mergeRequest.ToMergeRequestClient();
        }
    }

    private void SetLabels(MergeRequest mergeRequest, string labels, string labelsToAdd, string labelsToRemove)
    {
        if (labels is not null || labelsToAdd is not null || labelsToRemove is not null)
        {
            var newLabels = mergeRequest.Labels.ToArray();
            if (labels is not null)
            {
                newLabels = labels.Split(',').Distinct(StringComparer.Ordinal).ToArray();
            }

            if (labelsToAdd is not null)
            {
                newLabels = newLabels.Concat(labelsToAdd.Split(',')).Distinct(StringComparer.Ordinal).ToArray();
            }

            if (labelsToRemove is not null)
            {
                newLabels = newLabels.Except(labelsToRemove.Split(','), StringComparer.Ordinal).Distinct(StringComparer.Ordinal).ToArray();
            }

            if (newLabels is not null)
            {
                Server.ResourceLabelEvents.CreateResourceLabelEvents(Context.User, mergeRequest.Labels.ToArray(), newLabels, mergeRequest.Id, "MergeRequest");
            }

            mergeRequest.Labels.Clear();
            foreach (var newLabel in newLabels)
            {
                if (!string.IsNullOrEmpty(newLabel))
                {
                    mergeRequest.Labels.Add(newLabel);
                }
            }
        }
    }

    public void Delete(long mergeRequestIid)
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

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "There are lots of cases to support")]
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
                mergeRequests = mergeRequests.Where(mr => mr.CreatedAt >= query.CreatedAfter.Value.ToDateTimeOffsetAssumeUtc());
            }

            if (query.CreatedBefore != null)
            {
                mergeRequests = mergeRequests.Where(mr => mr.CreatedAt <= query.CreatedBefore.Value.ToDateTimeOffsetAssumeUtc());
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
                mergeRequests = mergeRequests.Where(mr => mr.UpdatedAt >= query.UpdatedAfter.Value.ToDateTimeOffsetAssumeUtc());
            }

            if (query.UpdatedBefore != null)
            {
                mergeRequests = mergeRequests.Where(mr => mr.UpdatedAt <= query.UpdatedBefore.Value.ToDateTimeOffsetAssumeUtc());
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
                mergeRequests = mergeRequests.Where(mr => (bool)query.Wip ? mr.Draft : !mr.Draft);
            }
        }

        return mergeRequests.Select(mr => mr.ToMergeRequestClient()).ToList();
    }

    public IEnumerable<Author> GetParticipants(long mergeRequestIid)
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

    public IEnumerable<PipelineBasic> GetPipelines(long mergeRequestIid)
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

    public Models.MergeRequest Reopen(long mergeRequestIid)
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

            Server.ResourceStateEvents.CreateResourceStateEvent(Context.User, "reopened", mergeRequest.Id, "MergeRequest");
            return mergeRequest.ToMergeRequestClient();
        }
    }

    public Models.MergeRequest Update(long mergeRequestIid, MergeRequestUpdate mergeRequestUpdate)
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
                        var user = Server.Users.GetById(assigneeId);
                        if (user == null)
                            throw new GitLabBadRequestException("user not found");

                        mergeRequest.Assignees.Add(new UserRef(user));
                    }
                }
            }
            else if (mergeRequestUpdate.AssigneeId != null)
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

            if (mergeRequestUpdate.MilestoneId != null)
            {
                var prevMilestone = mergeRequest.Milestone;
                mergeRequest.Milestone = GetMilestone(project.Id, mergeRequestUpdate.MilestoneId.Value);
                Server.ResourceMilestoneEvents.CreateResourceMilestoneEvents(Context.User, mergeRequest.Id, prevMilestone, mergeRequest.Milestone, "MergeRequest");
            }

            if (mergeRequestUpdate.ReviewerIds != null)
            {
                foreach (var reviewerId in mergeRequestUpdate.ReviewerIds)
                {
                    var reviewer = Server.Users.GetById(reviewerId);
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

            SetLabels(mergeRequest, mergeRequestUpdate.Labels, mergeRequestUpdate.AddLabels, mergeRequestUpdate.RemoveLabels);

            mergeRequest.UpdatedAt = DateTimeOffset.UtcNow;
            return mergeRequest.ToMergeRequestClient();
        }
    }

    public IEnumerable<Models.Issue> ClosesIssues(long mergeRequestIid)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<MergeRequestVersion> GetVersionsAsync(long mergeRequestIid)
    {
        throw new NotImplementedException();
    }

    public Task<TimeStats> TimeStatsAsync(long mergeRequestIid, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IMergeRequestDiscussionClient Discussions(long mergeRequestIid)
    {
        AssertProjectId();

        return new MergeRequestDiscussionClient(Context, _projectId.GetValueOrDefault(), mergeRequestIid);
    }

    public GitLabCollectionResponse<Models.ResourceLabelEvent> ResourceLabelEventsAsync(long projectId, long mergeRequestIid)
    {
        using (Context.BeginOperationScope())
        {
            var mergeRequest = GetMergeRequest(projectId, mergeRequestIid);
            var resourceLabelEvents = Server.ResourceLabelEvents.Get(mergeRequest.Id);

            return GitLabCollectionResponse.Create(resourceLabelEvents.Select(rle => rle.ToClientResourceLabelEvent()));
        }
    }

    public GitLabCollectionResponse<Models.ResourceMilestoneEvent> ResourceMilestoneEventsAsync(long projectId, long mergeRequestIid)
    {
        using (Context.BeginOperationScope())
        {
            var mergeRequest = GetMergeRequest(projectId, mergeRequestIid);
            var resourceMilestoneEvents = Server.ResourceMilestoneEvents.Get(mergeRequest.Id);

            return GitLabCollectionResponse.Create(resourceMilestoneEvents.Select(rme => rme.ToClientResourceMilestoneEvent()));
        }
    }

    public GitLabCollectionResponse<Models.ResourceStateEvent> ResourceStateEventsAsync(long projectId, long mergeRequestIid)
    {
        using (Context.BeginOperationScope())
        {
            var mergeRequest = GetMergeRequest(projectId, mergeRequestIid);
            var resourceStateEvents = Server.ResourceStateEvents.Get(mergeRequest.Id);

            return GitLabCollectionResponse.Create(resourceStateEvents.Select(rle => rle.ToClientResourceStateEvent()));
        }
    }
}
