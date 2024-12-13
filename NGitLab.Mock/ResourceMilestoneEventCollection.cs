using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ResourceMilestoneEventCollection : Collection<ResourceMilestoneEvent>
{
    public ResourceMilestoneEventCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(ResourceMilestoneEvent item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = GetNewId();
        }

        base.Add(item);
    }

    internal IEnumerable<ResourceMilestoneEvent> Get(long? resourceId)
    {
        var resourceMilestoneEvents = this.AsQueryable();

        if (resourceId.HasValue)
        {
            resourceMilestoneEvents = resourceMilestoneEvents.Where(rle => rle.ResourceId == resourceId);
        }

        return resourceMilestoneEvents;
    }

    private long GetNewId()
    {
        return this.Select(rle => rle.Id).DefaultIfEmpty().Max() + 1;
    }

    internal void CreateResourceMilestoneEvents(User currentUser, long resourceId, Milestone previousMilestone, Milestone newMilestone, string resourceType)
    {
        if (previousMilestone is null)
        {
            CreateResourceMilestoneEvent(currentUser, resourceId, newMilestone, ResourceMilestoneEventAction.Add, resourceType);
        }
        else if (newMilestone is not null && previousMilestone is not null)
        {
            if (newMilestone.Id != previousMilestone.Id)
            {
                CreateResourceMilestoneEvent(currentUser, resourceId, previousMilestone, ResourceMilestoneEventAction.Remove, resourceType);
            }

            CreateResourceMilestoneEvent(currentUser, resourceId, newMilestone, ResourceMilestoneEventAction.Add, resourceType);
        }
    }

    internal void CreateResourceMilestoneEvent(User currentUser, long resourceId, Milestone milestone, ResourceMilestoneEventAction action, string resourceType)
    {
        Add(new ResourceMilestoneEvent()
        {
            Action = action,
            Milestone = milestone,
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
