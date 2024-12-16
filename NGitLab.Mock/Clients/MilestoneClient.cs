using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NGitLab.Impl;
using NGitLab.Mock.Clients;
using NGitLab.Models;

namespace NGitLab.Mock;

internal sealed class MilestoneClient : ClientBase, IMilestoneClient
{
    private readonly long _resourceId;

    public MilestoneClient(ClientContext context, IIdOrPathAddressable id, MilestoneScope scope)
        : base(context)
    {
        _resourceId = scope switch
        {
            MilestoneScope.Groups => Server.AllGroups.FindGroup(id.ValueAsString()).Id,
            MilestoneScope.Projects => Server.AllProjects.FindProject(id.ValueAsString()).Id,
            _ => throw new NotSupportedException($"{scope} milestone is not supported yet."),
        };
        Scope = scope;
    }

    public Models.Milestone this[long id]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return GetMilestone(id, false).ToClientMilestone();
            }
        }
    }

    public IEnumerable<Models.Milestone> All => Get(new MilestoneQuery());

    public MilestoneScope Scope { get; }

    public Models.Milestone Activate(long milestoneId)
    {
        using (Context.BeginOperationScope())
        {
            var milestone = GetMilestone(milestoneId, true);
            milestone.State = MilestoneState.active;
            return milestone.ToClientMilestone();
        }
    }

    public IEnumerable<Models.MergeRequest> GetMergeRequests(long milestoneId)
    {
        using (Context.BeginOperationScope())
        {
            var milestone = GetMilestone(milestoneId, false);
            IEnumerable<MergeRequest> mergeRequests;

            switch (Scope)
            {
                case MilestoneScope.Groups:
                    mergeRequests = milestone.Group.MergeRequests;
                    break;
                case MilestoneScope.Projects:
                    mergeRequests = milestone.Project.MergeRequests;
                    break;
                default:
                    throw new NotSupportedException($"{Scope} milestone is not supported yet.");
            }

            mergeRequests = mergeRequests.Where(mr => mr.Milestone == milestone);
            return mergeRequests.Select(mr => mr.ToMergeRequestClient());
        }
    }

    public IEnumerable<Models.Milestone> AllInState(Models.MilestoneState state)
    {
        return Get(new MilestoneQuery { State = state });
    }

    public Models.Milestone Close(long milestoneId)
    {
        using (Context.BeginOperationScope())
        {
            var milestone = GetMilestone(milestoneId, true);
            milestone.State = MilestoneState.closed;
            return milestone.ToClientMilestone();
        }
    }

    public Models.Milestone Create(MilestoneCreate milestone)
    {
        using (Context.BeginOperationScope())
        {
            var ms = new Milestone
            {
                Title = milestone.Title,
                Description = milestone.Description,
                DueDate = string.IsNullOrEmpty(milestone.DueDate) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(milestone.DueDate),
                StartDate = string.IsNullOrEmpty(milestone.StartDate) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(milestone.StartDate),
            };

            switch (Scope)
            {
                case MilestoneScope.Groups:
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    group.Milestones.Add(ms);
                    break;
                case MilestoneScope.Projects:
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    project.Milestones.Add(ms);
                    break;
                default:
                    throw new NotSupportedException($"{Scope} milestone is not supported yet.");
            }

            return ms.ToClientMilestone();
        }
    }

    public void Delete(long milestoneId)
    {
        using (Context.BeginOperationScope())
        {
            var milestone = GetMilestone(milestoneId, true);
            switch (Scope)
            {
                case MilestoneScope.Groups:
                    milestone.Group.Milestones.Remove(milestone);
                    break;
                case MilestoneScope.Projects:
                    milestone.Project.Milestones.Remove(milestone);
                    break;
                default:
                    throw new NotSupportedException($"{Scope} milestone is not supported yet.");
            }
        }
    }

    public IEnumerable<Models.Milestone> Get(MilestoneQuery query)
    {
        using (Context.BeginOperationScope())
        {
            IEnumerable<Milestone> milestones;

            switch (Scope)
            {
                case MilestoneScope.Groups:
                    var group = GetGroup(_resourceId, GroupPermission.View);
                    milestones = group.Milestones;
                    break;
                case MilestoneScope.Projects:
                    var project = GetProject(_resourceId, ProjectPermission.View);
                    milestones = project.Milestones;
                    break;
                default:
                    throw new NotSupportedException($"{Scope} milestone is not supported yet.");
            }

            if (query.State != null)
            {
                milestones = milestones.Where(m => (int)m.State == (int)query.State);
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                milestones = milestones.Where(m => m.Title.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
            }

            return milestones.Select(m => m.ToClientMilestone());
        }
    }

    public Models.Milestone Update(long milestoneId, MilestoneUpdate milestone)
    {
        using (Context.BeginOperationScope())
        {
            var ms = GetMilestone(milestoneId, true);

            if (!string.IsNullOrEmpty(milestone.Title))
            {
                ms.Title = milestone.Title;
            }

            if (milestone.Description != null)
            {
                ms.Description = milestone.Description;
            }

            if (!string.IsNullOrEmpty(milestone.DueDate))
            {
                ms.DueDate = DateTimeOffset.Parse(milestone.DueDate, CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(milestone.StartDate))
            {
                ms.StartDate = DateTimeOffset.Parse(milestone.StartDate, CultureInfo.InvariantCulture);
            }

            return ms.ToClientMilestone();
        }
    }

    private Milestone GetMilestone(long milestoneId, bool editing)
    {
        Milestone milestone;

        switch (Scope)
        {
            case MilestoneScope.Groups:
                var group = GetGroup(_resourceId, editing ? GroupPermission.Edit : GroupPermission.View);
                milestone = group.Milestones.FirstOrDefault(x => x.Id == milestoneId);
                break;
            case MilestoneScope.Projects:
                var project = GetProject(_resourceId, editing ? ProjectPermission.Edit : ProjectPermission.View);
                milestone = project.Milestones.FirstOrDefault(x => x.Id == milestoneId);
                break;
            default:
                throw new NotSupportedException($"{Scope} milestone is not supported yet.");
        }

        return milestone ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
    }
}
