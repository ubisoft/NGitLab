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

    Milestone this[int id] { get; }

    Milestone Create(MilestoneCreate milestone);

    Milestone Update(int milestoneId, MilestoneUpdate milestone);

    void Delete(int milestoneId);

    Milestone Close(int milestoneId);

    Milestone Activate(int milestoneId);

    IEnumerable<MergeRequest> GetMergeRequests(int milestoneId);
}
