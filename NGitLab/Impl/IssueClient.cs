﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

public class IssueClient : IIssueClient
{
    private const string IssuesUrl = "/issues";
    private const string IssueByIdUrl = "/issues/{0}";
    private const string LinkedIssuesByIdUrl = "/projects/{0}/issues/{1}/links";
    private const string CreateLinkBetweenIssuesUrl = "/projects/{0}/issues/{1}/links?target_project_id={2}&target_issue_iid={3}";
    private const string GroupIssuesUrl = "/groups/{0}/issues";
    private const string ProjectIssuesUrl = "/projects/{0}/issues";
    private const string SingleIssueUrl = "/projects/{0}/issues/{1}";
    private const string ResourceLabelEventUrl = "/projects/{0}/issues/{1}/resource_label_events";
    private const string ResourceMilestoneEventUrl = "/projects/{0}/issues/{1}/resource_milestone_events";
    private const string ResourceStateEventUrl = "/projects/{0}/issues/{1}/resource_state_events";
    private const string RelatedToUrl = "/projects/{0}/issues/{1}/related_merge_requests";
    private const string ClosedByUrl = "/projects/{0}/issues/{1}/closed_by";
    private const string TimeStatsUrl = "/projects/{0}/issues/{1}/time_stats";
    private const string CloneIssueUrl = "/projects/{0}/issues/{1}/clone";
    private const string ParticipantsUrl = "/projects/{0}/issues/{1}/participants";
    private const string SubscribeUrl = "/projects/{0}/issues/{1}/subscribe";
    private const string UnsubscribeUrl = "/projects/{0}/issues/{1}/unsubscribe";

    private readonly API _api;

    public IssueClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Issue> Owned => _api.Get().GetAll<Issue>(IssuesUrl);

    public IEnumerable<Issue> ForProject(long projectId)
    {
        return _api.Get().GetAll<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId));
    }

    public GitLabCollectionResponse<Issue> ForProjectAsync(long projectId)
    {
        return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId));
    }

    public GitLabCollectionResponse<Issue> ForProjectAsync(long projectId, IssueQuery query)
    {
        return Get(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId), query);
    }

    public GitLabCollectionResponse<Issue> ForGroupsAsync(long groupId)
    {
        return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, GroupIssuesUrl, groupId));
    }

    public GitLabCollectionResponse<Issue> ForGroupsAsync(long groupId, IssueQuery query)
    {
        return Get(string.Format(CultureInfo.InvariantCulture, GroupIssuesUrl, groupId), query);
    }

    public Issue Get(long projectId, long issueIid)
    {
        return _api.Get().To<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, projectId, issueIid));
    }

    public Task<Issue> GetAsync(long projectId, long issueIid, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, projectId, issueIid), cancellationToken);
    }

    public IEnumerable<Issue> Get(long projectId, IssueQuery query)
    {
        return Get(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId), query);
    }

    public GitLabCollectionResponse<Issue> GetAsync(long projectId, IssueQuery query)
    {
        return Get(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId), query);
    }

    public IEnumerable<Issue> Get(IssueQuery query)
    {
        return Get(IssuesUrl, query);
    }

    public GitLabCollectionResponse<Issue> GetAsync(IssueQuery query)
    {
        return Get(IssuesUrl, query);
    }

    public Issue GetById(long issueId)
    {
        return _api.Get().To<Issue>(string.Format(CultureInfo.InvariantCulture, IssueByIdUrl, issueId));
    }

    public Task<Issue> GetByIdAsync(long issueId, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, IssueByIdUrl, issueId), cancellationToken);
    }

    public Issue Create(IssueCreate issueCreate)
    {
        return _api.Post().With(issueCreate).To<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, issueCreate.ProjectId));
    }

    public Task<Issue> CreateAsync(IssueCreate issueCreate, CancellationToken cancellationToken = default)
    {
        return _api.Post().With(issueCreate).ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, issueCreate.ProjectId), cancellationToken);
    }

    public Issue Edit(IssueEdit issueEdit)
    {
        return _api.Put().With(issueEdit).To<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, issueEdit.ProjectId, issueEdit.IssueId));
    }

    public Task<Issue> EditAsync(IssueEdit issueEdit, CancellationToken cancellationToken = default)
    {
        return _api.Put().With(issueEdit).ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, issueEdit.ProjectId, issueEdit.IssueId), cancellationToken);
    }

    public IEnumerable<ResourceLabelEvent> ResourceLabelEvents(long projectId, long issueIid)
    {
        return _api.Get().GetAll<ResourceLabelEvent>(string.Format(CultureInfo.InvariantCulture, ResourceLabelEventUrl, projectId, issueIid));
    }

    public GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(long projectId, long issueIid)
    {
        return _api.Get().GetAllAsync<ResourceLabelEvent>(string.Format(CultureInfo.InvariantCulture, ResourceLabelEventUrl, projectId, issueIid));
    }

    public IEnumerable<ResourceMilestoneEvent> ResourceMilestoneEvents(long projectId, long issueIid)
    {
        return _api.Get().GetAll<ResourceMilestoneEvent>(string.Format(CultureInfo.InvariantCulture, ResourceMilestoneEventUrl, projectId, issueIid));
    }

    public GitLabCollectionResponse<ResourceMilestoneEvent> ResourceMilestoneEventsAsync(long projectId, long issueIid)
    {
        return _api.Get().GetAllAsync<ResourceMilestoneEvent>(string.Format(CultureInfo.InvariantCulture, ResourceMilestoneEventUrl, projectId, issueIid));
    }

    public IEnumerable<ResourceStateEvent> ResourceStateEvents(long projectId, long issueIid)
    {
        return _api.Get().GetAll<ResourceStateEvent>(string.Format(CultureInfo.InvariantCulture, ResourceStateEventUrl, projectId, issueIid));
    }

    public GitLabCollectionResponse<ResourceStateEvent> ResourceStateEventsAsync(long projectId, long issueIid)
    {
        return _api.Get().GetAllAsync<ResourceStateEvent>(string.Format(CultureInfo.InvariantCulture, ResourceStateEventUrl, projectId, issueIid));
    }

    public IEnumerable<MergeRequest> RelatedTo(long projectId, long issueIid)
    {
        return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, RelatedToUrl, projectId, issueIid));
    }

    public GitLabCollectionResponse<Issue> LinkedToAsync(long projectId, long issueId)
    {
        return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, LinkedIssuesByIdUrl, projectId, issueId));
    }

    public bool CreateLinkBetweenIssues(long sourceProjectId, long sourceIssueId, long targetProjectId,
        long targetIssueId)
    {
        try
        {
            _api.Post().Execute(string.Format(CultureInfo.InvariantCulture, CreateLinkBetweenIssuesUrl, sourceProjectId, sourceIssueId, targetProjectId, targetIssueId));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public GitLabCollectionResponse<MergeRequest> RelatedToAsync(long projectId, long issueIid)
    {
        return _api.Get().GetAllAsync<MergeRequest>(string.Format(CultureInfo.InvariantCulture, RelatedToUrl, projectId, issueIid));
    }

    public IEnumerable<MergeRequest> ClosedBy(long projectId, long issueIid)
    {
        return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, ClosedByUrl, projectId, issueIid));
    }

    public GitLabCollectionResponse<MergeRequest> ClosedByAsync(long projectId, long issueIid)
    {
        return _api.Get().GetAllAsync<MergeRequest>(string.Format(CultureInfo.InvariantCulture, ClosedByUrl, projectId, issueIid));
    }

    public Task<TimeStats> TimeStatsAsync(long projectId, long issueIid, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<TimeStats>(string.Format(CultureInfo.InvariantCulture, TimeStatsUrl, projectId, issueIid), cancellationToken);
    }

    public Task<Issue> CloneAsync(long projectId, long issueIid, IssueClone issueClone, CancellationToken cancellationToken = default)
    {
        return _api.Post().With(issueClone).ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, CloneIssueUrl, projectId, issueIid), cancellationToken);
    }

    public IEnumerable<Participant> GetParticipants(ProjectId projectId, long issueIid)
    {
        return _api.Get().GetAll<Participant>(string.Format(CultureInfo.InvariantCulture, ParticipantsUrl, projectId.ValueAsUriParameter(), issueIid));
    }

    public Issue Subscribe(ProjectId projectId, long issueIid)
    {
        return _api.Post().To<Issue>(string.Format(CultureInfo.InvariantCulture, SubscribeUrl, projectId.ValueAsUriParameter(), issueIid));
    }

    public Issue Unsubscribe(ProjectId projectId, long issueIid)
    {
        return _api.Post().To<Issue>(string.Format(CultureInfo.InvariantCulture, UnsubscribeUrl, projectId.ValueAsUriParameter(), issueIid));
    }

    private GitLabCollectionResponse<Issue> Get(string url, IssueQuery query)
    {
        url = AddIssueQueryParameters(url, query);
        return _api.Get().GetAllAsync<Issue>(url);
    }

    private static string AddIssueQueryParameters(string url, IssueQuery query)
    {
        url = Utils.AddParameter(url, "state", query.State);
        url = Utils.AddParameter(url, "issue_type", query.Type);
        url = Utils.AddParameter(url, "order_by", query.OrderBy);
        url = Utils.AddParameter(url, "sort", query.Sort);
        url = Utils.AddParameter(url, "milestone", query.Milestone);
        url = Utils.AddParameter(url, "labels", query.Labels);
        url = Utils.AddParameter(url, "created_after", query.CreatedAfter);
        url = Utils.AddParameter(url, "created_before", query.CreatedBefore);
        url = Utils.AddParameter(url, "updated_after", query.UpdatedAfter);
        url = Utils.AddParameter(url, "updated_before", query.UpdatedBefore);
        url = Utils.AddParameter(url, "confidential", query.Confidential);
        url = Utils.AddParameter(url, "scope", query.Scope);
        url = Utils.AddParameter(url, "author_id", query.AuthorId);
        url = Utils.AddParameter(url, "per_page", query.PerPage);
        url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
        url = Utils.AddParameter(url, "search", query.Search);
        return url;
    }
}
