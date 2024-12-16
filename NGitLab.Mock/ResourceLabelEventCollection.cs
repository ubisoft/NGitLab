using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ResourceLabelEventCollection : Collection<ResourceLabelEvent>
{
    public ResourceLabelEventCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(ResourceLabelEvent item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = GetNewId();
        }

        base.Add(item);
    }

    internal IEnumerable<ResourceLabelEvent> Get(long? resourceId)
    {
        var resourceLabelEvents = this.AsQueryable();

        if (resourceId.HasValue)
        {
            resourceLabelEvents = resourceLabelEvents.Where(rle => rle.ResourceId == resourceId);
        }

        return resourceLabelEvents;
    }

    private long GetNewId()
    {
        return this.Select(rle => rle.Id).DefaultIfEmpty().Max() + 1;
    }

    internal void CreateResourceLabelEvents(User currentUser, string[] previousLabels, string[] newLabels, long resourceId, string resourceType)
    {
        foreach (var label in previousLabels)
        {
            if (!newLabels.Any(l => string.Equals(l, label, StringComparison.OrdinalIgnoreCase)))
            {
                Add(new ResourceLabelEvent()
                {
                    Action = ResourceLabelEventAction.Remove,
                    Label = new Label() { Name = label },
                    ResourceId = resourceId,
                    CreatedAt = DateTime.UtcNow,
                    Id = Server.GetNewResourceLabelEventId(),
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

        foreach (var label in newLabels)
        {
            if (!previousLabels.Any(l => string.Equals(l, label, StringComparison.OrdinalIgnoreCase)))
            {
                Add(new ResourceLabelEvent()
                {
                    Action = ResourceLabelEventAction.Add,
                    Label = new Label() { Name = label },
                    ResourceId = resourceId,
                    CreatedAt = DateTime.UtcNow,
                    Id = Server.GetNewResourceLabelEventId(),
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
    }
}
