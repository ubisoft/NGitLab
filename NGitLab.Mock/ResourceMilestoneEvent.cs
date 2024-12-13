using System;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ResourceMilestoneEvent : GitLabObject
{
    public long Id { get; set; }

    public Author User { get; set; }

    public DateTime CreatedAt { get; set; }

    public long ResourceId { get; set; }

    public string ResourceType { get; set; }

    public Milestone Milestone { get; set; }

    public ResourceMilestoneEventAction Action { get; set; }

    public Models.ResourceMilestoneEvent ToClientResourceMilestoneEvent()
    {
        return new Models.ResourceMilestoneEvent()
        {
            Id = Id,
            User = User,
            CreatedAt = CreatedAt,
            ResourceId = ResourceId,
            ResourceType = ResourceType,
            Milestone = Milestone.ToClientMilestone(),
            Action = Action,
        };
    }
}
