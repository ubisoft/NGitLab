using System.Collections.Generic;
using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab;

public interface IMilestoneClient
{
    MilestoneScope Scope { get; }

    IEnumerable<Milestone> All { get; }

    IEnumerable<Milestone> AllInState(MilestoneState state);

    IEnumerable<Milestone> Get(MilestoneQuery query);

    Milestone this[long id] { get; }

    Milestone Create(MilestoneCreate milestone);

    Milestone Update(long milestoneId, MilestoneUpdate milestone);

    void Delete(long milestoneId);

    Milestone Close(long milestoneId);

    Milestone Activate(long milestoneId);

    IEnumerable<MergeRequest> GetMergeRequests(long milestoneId);
}
