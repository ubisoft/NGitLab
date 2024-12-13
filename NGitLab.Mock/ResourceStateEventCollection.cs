using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ResourceStateEventCollection : Collection<ResourceStateEvent>
{
    public ResourceStateEventCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(ResourceStateEvent item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = GetNewId();
        }

        base.Add(item);
    }

    internal IEnumerable<ResourceStateEvent> Get(long? resourceId)
    {
        var resourceStateEvents = this.AsQueryable();

        if (resourceId.HasValue)
        {
            resourceStateEvents = resourceStateEvents.Where(rle => rle.ResourceId == resourceId);
        }

        return resourceStateEvents;
    }

    private long GetNewId()
    {
        return this.Select(rle => rle.Id).DefaultIfEmpty().Max() + 1;
    }

    internal void CreateResourceStateEvent(User currentUser, string state, long id, string resourceType)
    {
        Add(new ResourceStateEvent()
        {
            ResourceId = id,
            CreatedAt = DateTime.UtcNow,
            Id = Server.GetNewResourceLabelEventId(),
            State = state,
            User = new Author()
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                AvatarUrl = currentUser.AvatarUrl,
                Name = currentUser.Name,
                State = currentUser.State.ToString(),
                Username = currentUser.UserName,
                CreatedAt = currentUser.CreatedAt,
                WebUrl = currentUser.WebUrl,
            },
            ResourceType = resourceType,
        });
    }
}
