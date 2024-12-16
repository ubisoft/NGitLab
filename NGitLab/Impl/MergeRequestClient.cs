using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class MergeRequestClient : IMergeRequestClient
{
    private const string ResourceLabelEventUrl = "/projects/{0}/merge_requests/{1}/resource_label_events";
    private const string ResourceMilestoneEventUrl = "/projects/{0}/merge_requests/{1}/resource_milestone_events";
    private const string ResourceStateEventUrl = "/projects/{0}/merge_requests/{1}/resource_state_events";
    private readonly string _path;
    private readonly API _api;

    public MergeRequestClient(API api, ProjectId projectId)
    {
        _api = api;
        _path = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
    }

    public MergeRequestClient(API api, GroupId groupId)
    {
        _api = api;
        _path = $"{Group.Url}/{groupId.ValueAsUriParameter()}";
    }

    public MergeRequestClient(API api)
    {
        _api = api;
        _path = string.Empty;
    }

    public IEnumerable<MergeRequest> All => Get(new MergeRequestQuery());

    public IEnumerable<MergeRequest> AllInState(MergeRequestState state) => Get(new MergeRequestQuery { State = state });

    public IEnumerable<MergeRequest> Get(MergeRequestQuery query)
    {
        var url = _path + MergeRequest.Url;

        url = Utils.AddParameter(url, "state", query.State);
        url = Utils.AddParameter(url, "order_by", query.OrderBy);
        url = Utils.AddParameter(url, "sort", query.Sort);
        url = Utils.AddParameter(url, "milestone", query.Milestone);
        url = Utils.AddParameter(url, "view", query.View);
        url = Utils.AddParameter(url, "labels", query.Labels);
        url = Utils.AddParameter(url, "created_after", query.CreatedAfter);
        url = Utils.AddParameter(url, "created_before", query.CreatedBefore);
        url = Utils.AddParameter(url, "updated_after", query.UpdatedAfter);
        url = Utils.AddParameter(url, "updated_before", query.UpdatedBefore);
        url = Utils.AddParameter(url, "scope", query.Scope);
        url = Utils.AddParameter(url, "author_id", query.AuthorId);
        url = Utils.AddParameter(url, "per_page", query.PerPage);
        url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
        url = Utils.AddParameter(url, "reviewer_id", query.ReviewerId);
        url = Utils.AddParameter(url, "approver_ids[]", query.ApproverIds);
        url = Utils.AddParameter(url, "source_branch", query.SourceBranch);
        url = Utils.AddParameter(url, "target_branch", query.TargetBranch);
        url = Utils.AddParameter(url, "search", query.Search);
        url = Utils.AddParameter(url, "wip", query.Wip.HasValue ? (query.Wip.Value ? "yes" : "no") : null);

        return _api.Get().GetAll<MergeRequest>(url);
    }

    public MergeRequest this[long iid]
    {
        get
        {
            var url = $"{_path}{MergeRequest.Url}/{iid.ToStringInvariant()}";
            url = Utils.AddParameter(url, "include_rebase_in_progress", value: true);
            url = Utils.AddParameter(url, "include_diverged_commits_count", value: true);

            return _api.Get().To<MergeRequest>(url);
        }
    }

    public Task<MergeRequest> GetByIidAsync(long iid, SingleMergeRequestQuery options, CancellationToken cancellationToken = default)
    {
        var url = $"{_path}{MergeRequest.Url}/{iid.ToStringInvariant()}";
        if (options != null)
        {
            url = Utils.AddParameter(url, "include_rebase_in_progress", options.IncludeRebaseInProgress);
            url = Utils.AddParameter(url, "include_diverged_commits_count", options.IncludeDivergedCommitsCount);
            url = Utils.AddParameter(url, "render_html", options.RenderHtml);
        }

        return _api.Get().ToAsync<MergeRequest>(url, cancellationToken);
    }

    public MergeRequest Create(MergeRequestCreate mergeRequest)
    {
        if (mergeRequest == null)
            throw new ArgumentNullException(nameof(mergeRequest));

        return _api
            .Post().With(mergeRequest)
            .To<MergeRequest>(_path + "/merge_requests");
    }

    public MergeRequest Update(long mergeRequestIid, MergeRequestUpdate mergeRequest) => _api
        .Put().With(mergeRequest)
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture));

    public MergeRequest Close(long mergeRequestIid) => _api
        .Put().With(new MergeRequestUpdateState { NewState = nameof(MergeRequestStateEvent.close) })
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture));

    public MergeRequest Reopen(long mergeRequestIid) => _api
        .Put().With(new MergeRequestUpdateState { NewState = nameof(MergeRequestStateEvent.reopen) })
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture));

    public void Delete(long mergeRequestIid) => _api
        .Delete()
        .Execute(_path + "/merge_requests/" + mergeRequestIid.ToStringInvariant());

    public MergeRequest CancelMergeWhenPipelineSucceeds(long mergeRequestIid) => _api
        .Post()
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/cancel_merge_when_pipeline_succeeds");

    public MergeRequest Accept(long mergeRequestIid, MergeRequestAccept message) => _api
        .Put().With(message)
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/merge");

    public MergeRequest Accept(long mergeRequestIid, MergeRequestMerge message) => _api
        .Put().With(message)
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/merge");

    public MergeRequest Approve(long mergeRequestIid, MergeRequestApprove message) => _api
        .Post().With(message)
        .To<MergeRequest>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/approve");

    public RebaseResult Rebase(long mergeRequestIid) => _api
        .Put()
        .To<RebaseResult>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/rebase");

    public Task<RebaseResult> RebaseAsync(long mergeRequestIid, MergeRequestRebase options, CancellationToken cancellationToken = default) => _api
        .Put().With(options)
        .ToAsync<RebaseResult>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/rebase", cancellationToken);

    public IEnumerable<PipelineBasic> GetPipelines(long mergeRequestIid)
    {
        return _api.Get().GetAll<PipelineBasic>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/pipelines");
    }

    public IEnumerable<Author> GetParticipants(long mergeRequestIid)
    {
        return _api.Get().GetAll<Author>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/participants");
    }

    public IEnumerable<Issue> ClosesIssues(long mergeRequestIid)
    {
        return _api.Get().GetAll<Issue>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/closes_issues");
    }

    public GitLabCollectionResponse<MergeRequestVersion> GetVersionsAsync(long mergeRequestIid)
    {
        return _api.Get().GetAllAsync<MergeRequestVersion>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/versions");
    }

    public Task<TimeStats> TimeStatsAsync(long mergeRequestIid, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<TimeStats>(_path + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/time_stats", cancellationToken);
    }

    public IMergeRequestCommentClient Comments(long mergeRequestIid) => new MergeRequestCommentClient(_api, _path, mergeRequestIid);

    public IMergeRequestDiscussionClient Discussions(long mergeRequestIid) => new MergeRequestDiscussionClient(_api, _path, mergeRequestIid);

    public IMergeRequestCommitClient Commits(long mergeRequestIid) => new MergeRequestCommitClient(_api, _path, mergeRequestIid);

    public IMergeRequestApprovalClient ApprovalClient(long mergeRequestIid) => new MergeRequestApprovalClient(_api, _path, mergeRequestIid);

    public IMergeRequestChangeClient Changes(long mergeRequestIid) => new MergeRequestChangeClient(_api, _path, mergeRequestIid);

    public GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(long projectId, long mergeRequestIid)
    {
        return _api.Get().GetAllAsync<ResourceLabelEvent>(string.Format(CultureInfo.InvariantCulture, ResourceLabelEventUrl, projectId, mergeRequestIid));
    }

    public GitLabCollectionResponse<ResourceMilestoneEvent> ResourceMilestoneEventsAsync(long projectId, long mergeRequestIid)
    {
        return _api.Get().GetAllAsync<ResourceMilestoneEvent>(string.Format(CultureInfo.InvariantCulture, ResourceMilestoneEventUrl, projectId, mergeRequestIid));
    }

    public GitLabCollectionResponse<ResourceStateEvent> ResourceStateEventsAsync(long projectId, long mergeRequestIid)
    {
        return _api.Get().GetAllAsync<ResourceStateEvent>(string.Format(CultureInfo.InvariantCulture, ResourceStateEventUrl, projectId, mergeRequestIid));
    }
}
