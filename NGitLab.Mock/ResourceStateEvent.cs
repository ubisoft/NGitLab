using System;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ResourceStateEvent : GitLabObject
{
    public long Id { get; set; }

    public Author User { get; set; }

    public DateTime CreatedAt { get; set; }

    public long ResourceId { get; set; }

    public string ResourceType { get; set; }

    public string State { get; set; }

    public Models.ResourceStateEvent ToClientResourceStateEvent()
    {
        return new Models.ResourceStateEvent()
        {
            Id = Id,
            User = User,
            CreatedAt = CreatedAt,
            ResourceId = ResourceId,
            ResourceType = ResourceType,
            State = State,
        };
    }
}
