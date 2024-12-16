using System;
using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class MilestoneClient : IMilestoneClient
{
    public MilestoneScope Scope { get; }

    private readonly API _api;
    private readonly string _milestonePath;

    internal MilestoneClient(API api, MilestoneScope scope, IIdOrPathAddressable id)
    {
        _api = api;
        _milestonePath = $"/{scope.ToString().ToLowerInvariant()}/{id.ValueAsUriParameter()}/milestones";
        Scope = scope;
    }

    [Obsolete("Use GitLabClient.GetMilestone() or GitLabClient.GetGroupMilestone() instead.")]
    public MilestoneClient(API api, long projectId)
        : this(api, MilestoneScope.Projects, (ProjectId)projectId)
    {
    }

    public IEnumerable<Milestone> All => Get(new MilestoneQuery());

    public IEnumerable<Milestone> AllInState(MilestoneState state) => Get(new MilestoneQuery { State = state });

    public IEnumerable<Milestone> Get(MilestoneQuery query)
    {
        var url = _milestonePath;

        url = Utils.AddParameter(url, "state", query.State);
        url = Utils.AddParameter(url, "search", query.Search);

        return _api.Get().GetAll<Milestone>(url);
    }

    public Milestone this[long id] => _api.Get().To<Milestone>($"{_milestonePath}/{id.ToStringInvariant()}");

    public Milestone Create(MilestoneCreate milestone) => _api
        .Post().With(milestone)
        .To<Milestone>(_milestonePath);

    public Milestone Update(long milestoneId, MilestoneUpdate milestone) => _api
        .Put().With(milestone)
        .To<Milestone>($"{_milestonePath}/{milestoneId.ToStringInvariant()}");

    public Milestone Close(long milestoneId) => _api
        .Put().With(new MilestoneUpdateState { NewState = nameof(MilestoneStateEvent.close) })
        .To<Milestone>($"{_milestonePath}/{milestoneId.ToStringInvariant()}");

    public Milestone Activate(long milestoneId) => _api
        .Put().With(new MilestoneUpdateState { NewState = nameof(MilestoneStateEvent.activate) })
        .To<Milestone>($"{_milestonePath}/{milestoneId.ToStringInvariant()}");

    public IEnumerable<MergeRequest> GetMergeRequests(long milestoneId) => _api
        .Get().GetAll<MergeRequest>($"{_milestonePath}/{milestoneId.ToStringInvariant()}/merge_requests");

    public void Delete(long milestoneId) => _api
        .Delete()
        .Execute($"{_milestonePath}/{milestoneId.ToStringInvariant()}");
}
