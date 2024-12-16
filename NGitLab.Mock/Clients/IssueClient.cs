using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class IssueClient : ClientBase, IIssueClient
{
    public IssueClient(ClientContext context)
        : base(context)
    {
    }

    public IEnumerable<Models.Issue> Owned
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var viewableProjects = Server.AllProjects.Where(p => p.CanUserViewProject(Context.User));
                var allIssues = viewableProjects.SelectMany(p => p.Issues);
                var assignedOrAuthoredIssues = allIssues.Where(i => i.CanUserViewIssue(Context.User));
                return assignedOrAuthoredIssues.Select(i => i.ToClientIssue()).ToList();
            }
        }
    }

    public Models.Issue Create(IssueCreate issueCreate)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(issueCreate.ProjectId, ProjectPermission.View);

            var issue = new Issue
            {
                Description = issueCreate.Description,
                Title = issueCreate.Title,
                Author = Context.User,
                Confidential = issueCreate.Confidential,
            };

            if (!string.IsNullOrEmpty(issueCreate.Labels))
            {
                issue.Labels = issueCreate.Labels.Split(',');
            }

            if (issueCreate.AssigneeId != null)
            {
                issue.Assignee = Server.Users.FirstOrDefault(u => u.Id == issueCreate.AssigneeId);
            }

            project.Issues.Add(issue);
            return project.Issues.First(i => i.Iid == issue.Iid).ToClientIssue();
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Issue> CreateAsync(IssueCreate issueCreate, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Create(issueCreate);
    }

    public Models.Issue Edit(IssueEdit issueEdit)
    {
        using (Context.BeginOperationScope())
        {
            var projectId = issueEdit.ProjectId;
            var issueToModify = GetIssue(projectId, issueEdit.IssueId);

            if (issueEdit.AssigneeId.HasValue)
            {
                issueToModify.Assignee = GetUser(issueEdit.AssigneeId.Value);
            }

            var prevMilestone = issueToModify.Milestone;

            if (issueEdit.MilestoneId.HasValue)
            {
                issueToModify.Milestone = GetMilestone(projectId, issueEdit.MilestoneId.Value);
                Server.ResourceMilestoneEvents.CreateResourceMilestoneEvents(Context.User, issueToModify.Id, prevMilestone, issueToModify.Milestone, "Issue");
            }

            issueToModify.Title = issueEdit.Title;
            issueToModify.Description = issueEdit.Description;

            string[] labelsEdit;

            if (issueEdit.Labels is null)
            {
                labelsEdit = null;
            }
            else if (string.Equals(issueEdit.Labels, string.Empty, StringComparison.Ordinal))
            {
                labelsEdit = Array.Empty<string>();
            }
            else
            {
                labelsEdit = issueEdit.Labels.Split(',');
            }

            if (labelsEdit is not null)
            {
                Server.ResourceLabelEvents.CreateResourceLabelEvents(Context.User, issueToModify.Labels, labelsEdit, issueToModify.Id, "issue");
                issueToModify.Labels = labelsEdit;
            }

            issueToModify.UpdatedAt = DateTimeOffset.UtcNow;
            var isValidState = Enum.TryParse<StateEvent>(issueEdit.State, out var requestedState);
            if (isValidState)
            {
                issueToModify.State = (IssueState)requestedState;
            }

            return issueToModify.ToClientIssue();
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Issue> EditAsync(IssueEdit issueEdit, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Edit(issueEdit);
    }

    public IEnumerable<Models.ResourceLabelEvent> ResourceLabelEvents(long projectId, long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(projectId, issueIid);
            return Server.ResourceLabelEvents.Get(issue.Id).Select(rle => rle.ToClientResourceLabelEvent());
        }
    }

    public GitLabCollectionResponse<Models.ResourceLabelEvent> ResourceLabelEventsAsync(long projectId, long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(projectId, issueIid);
            var resourceLabelEvents = Server.ResourceLabelEvents.Get(issue.Id);

            return GitLabCollectionResponse.Create(resourceLabelEvents.Select(rle => rle.ToClientResourceLabelEvent()));
        }
    }

    public IEnumerable<Models.ResourceMilestoneEvent> ResourceMilestoneEvents(long projectId, long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(projectId, issueIid);
            return Server.ResourceMilestoneEvents.Get(issue.Id).Select(rme => rme.ToClientResourceMilestoneEvent());
        }
    }

    public GitLabCollectionResponse<Models.ResourceMilestoneEvent> ResourceMilestoneEventsAsync(long projectId, long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(projectId, issueIid);
            var resourceMilestoneEvents = Server.ResourceMilestoneEvents.Get(issue.Id);

            return GitLabCollectionResponse.Create(resourceMilestoneEvents.Select(rme => rme.ToClientResourceMilestoneEvent()));
        }
    }

    public IEnumerable<Models.ResourceStateEvent> ResourceStateEvents(long projectId, long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(projectId, issueIid);
            return Server.ResourceStateEvents.Get(issue.Id).Select(rle => rle.ToClientResourceStateEvent());
        }
    }

    public GitLabCollectionResponse<Models.ResourceStateEvent> ResourceStateEventsAsync(long projectId, long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(projectId, issueIid);
            var resourceStateEvents = Server.ResourceStateEvents.Get(issue.Id);

            return GitLabCollectionResponse.Create(resourceStateEvents.Select(rle => rle.ToClientResourceStateEvent()));
        }
    }

    public IEnumerable<Models.MergeRequest> RelatedTo(long projectId, long issueId)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<Models.MergeRequest> RelatedToAsync(long projectId, long issueIid)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Models.MergeRequest> ClosedBy(long projectId, long issueId)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<Models.MergeRequest> ClosedByAsync(long projectId, long issueIid)
    {
        throw new NotImplementedException();
    }

    public Task<TimeStats> TimeStatsAsync(long projectId, long issueIid, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Models.Issue> CloneAsync(long projectId, long issueIid, IssueClone issueClone, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Models.Issue> ForProject(long projectId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);

            return project
                .Issues
                .Where(i => i.CanUserViewIssue(Context.User))
                .Select(i => i.ToClientIssue())
                .ToList();
        }
    }

    public GitLabCollectionResponse<Models.Issue> ForProjectAsync(long projectId)
    {
        return GitLabCollectionResponse.Create(ForProject(projectId));
    }

    public GitLabCollectionResponse<Models.Issue> ForGroupsAsync(long groupId)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<Models.Issue> ForGroupsAsync(long groupId, IssueQuery query)
    {
        throw new NotImplementedException();
    }

    public Models.Issue Get(long projectId, long issueId)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            return project.Issues.FirstOrDefault(i => i.Iid == issueId &&
                    i.CanUserViewIssue(Context.User))?
                    .ToClientIssue() ?? throw new GitLabNotFoundException();
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Issue> GetAsync(long projectId, long issueId, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Get(projectId, issueId);
    }

    public IEnumerable<Models.Issue> Get(IssueQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var viewableProjects = Server.AllProjects.Where(p => p.CanUserViewProject(Context.User));
            var issues = viewableProjects.SelectMany(p => p.Issues.Where(i => i.CanUserViewIssue(Context.User)));
            return FilterByQuery(issues, query).Select(i => i.ToClientIssue()).ToList();
        }
    }

    public GitLabCollectionResponse<Models.Issue> GetAsync(IssueQuery query)
    {
        return GitLabCollectionResponse.Create(Get(query));
    }

    public IEnumerable<Models.Issue> Get(long projectId, IssueQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var issues = project.Issues.Where(i => i.CanUserViewIssue(Context.User));
            return FilterByQuery(issues, query).Select(i => i.ToClientIssue()).ToList();
        }
    }

    public GitLabCollectionResponse<Models.Issue> GetAsync(long projectId, IssueQuery query)
    {
        return GitLabCollectionResponse.Create(Get(projectId, query));
    }

    public async Task<Models.Issue> GetByIdAsync(long issueId, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return GetById(issueId);
    }

    public GitLabCollectionResponse<Models.Issue> LinkedToAsync(long projectId, long issueId)
    {
        throw new NotImplementedException();
    }

    public bool CreateLinkBetweenIssues(long sourceProjectId, long sourceIssueId, long targetProjectId, long targetIssueId)
    {
        throw new NotImplementedException();
    }

    public Models.Issue GetById(long issueId)
    {
        using (Context.BeginOperationScope())
        {
            var viewableProjects = Server.AllProjects.Where(p => p.CanUserViewProject(Context.User));
            return viewableProjects
                .SelectMany(p => p.Issues.Where(i => i.CanUserViewIssue(Context.User) && i.Id == issueId))
                .FirstOrDefault()?
                .ToClientIssue() ?? throw new GitLabNotFoundException();
        }
    }

    private IEnumerable<Issue> FilterByQuery(IEnumerable<Issue> issues, IssueQuery query)
    {
        if (query.State != null)
        {
            var isValidState = Enum.TryParse<IssueState>(query.State.ToString(), out var requestedState);
            if (isValidState)
            {
                issues = issues.Where(i => i.State == requestedState);
            }
        }

        if (query.Milestone != null)
        {
            issues = issues.Where(i => string.Equals(i.Milestone?.Title, query.Milestone, StringComparison.Ordinal));
        }

        if (!string.IsNullOrEmpty(query.Labels))
        {
            foreach (var label in query.Labels.Split(','))
            {
                issues = issues.Where(i => i.Labels.Contains(label, StringComparer.Ordinal));
            }
        }

        if (query.CreatedAfter != null)
        {
            issues = issues.Where(i => i.CreatedAt > query.CreatedAfter);
        }

        if (query.CreatedBefore != null)
        {
            issues = issues.Where(i => i.CreatedAt < query.CreatedBefore);
        }

        if (query.UpdatedAfter != null)
        {
            issues = issues.Where(i => i.UpdatedAt > query.UpdatedAfter);
        }

        if (query.UpdatedBefore != null)
        {
            issues = issues.Where(i => i.UpdatedAt < query.UpdatedBefore);
        }

        if (query.Scope != null)
        {
            var userId = Context.User.Id;
            switch (query.Scope)
            {
                case "created_by_me":
                case "created-by-me":
                    issues = issues.Where(i => i.Author.Id == userId);
                    break;
                case "assigned_to_me":
                case "assigned-to-me":
                    issues = issues.Where(i => i.Assignees?.Any(x => x.Id == userId) == true);
                    break;
                case "all":
                    break;
                default:
                    throw new NotSupportedException($"Scope '{query.Scope}' is not supported");
            }
        }

        if (query.AuthorId != null)
        {
            issues = issues.Where(i => i.Author.Id == query.AuthorId);
        }

        if (query.UpdatedBefore != null)
        {
            issues = issues.Where(i => i.UpdatedAt < query.UpdatedBefore);
        }

        if (query.AssigneeId != null)
        {
            var isUserId = int.TryParse(query.AssigneeId.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId);

            if (isUserId)
            {
                issues = issues.Where(i => i.Assignee != null && i.Assignee.Id == userId);
            }
            else if (string.Equals(query.AssigneeId.ToString(), "None", StringComparison.OrdinalIgnoreCase))
            {
                issues = issues.Where(i => i.Assignee == null);
            }
        }

        if (query.Milestone != null)
        {
            issues = issues.Where(i => string.Equals(i.Milestone?.Title, query.Milestone, StringComparison.Ordinal));
        }

        if (query.Confidential != null)
        {
            issues = issues.Where(i => i.Confidential == query.Confidential.Value);
        }

        if (query.Search != null)
        {
            issues = issues
                .Where(i => i.Title.Contains(query.Search, StringComparison.OrdinalIgnoreCase)
                    || i.Description.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
        }

        if (query.PerPage != null)
        {
            issues = issues.Take(query.PerPage.Value);
        }

        if (query.OrderBy != null)
        {
            issues = query.OrderBy switch
            {
                "created_at" => issues.OrderBy(i => i.CreatedAt),
                "updated_at" => issues.OrderBy(i => i.UpdatedAt),
                _ => throw new NotSupportedException($"OrderBy '{query.OrderBy}' is not supported"),
            };
        }

        if (string.Equals(query.Sort, "asc", StringComparison.Ordinal))
        {
            issues = issues.Reverse();
        }

        return issues;
    }

    public IEnumerable<Participant> GetParticipants(ProjectId projectId, long issueIid)
    {
        throw new NotImplementedException();
    }

    public Models.Issue Unsubscribe(ProjectId projectId, long issueIid)
    {
        throw new NotImplementedException();
    }
}
