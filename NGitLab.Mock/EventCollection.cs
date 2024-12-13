using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class EventCollection : Collection<Event>
{
    public EventCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(Event item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = GetNewId();
        }

        base.Add(item);
    }

    internal IEnumerable<Event> Get(EventQuery query, long? userId, long? projectId)
    {
        var events = this.AsQueryable();

        if (userId.HasValue)
        {
            events = events.Where(e => e.AuthorId == userId);
        }

        if (projectId.HasValue)
        {
            events = events.Where(e => e.ProjectId == projectId);
        }

        if (query.Action.HasValue)
        {
            events = events.Where(u => u.Action == query.Action.Value);
        }

        if (query.After.HasValue)
        {
            events = events.Where(u => u.CreatedAt > query.After);
        }

        if (query.Before.HasValue)
        {
            events = events.Where(u => u.CreatedAt < query.Before);
        }

        if (query.Type.HasValue)
        {
            events = events.Where(u => u.TargetType == query.Type.Value);
        }

        if (!string.IsNullOrEmpty(query.Sort))
        {
            var sortAsc = !string.IsNullOrEmpty(query.Sort) && string.Equals(query.Sort, "asc", StringComparison.Ordinal);
            events = sortAsc ? events.OrderBy(e => e.CreatedAt) : events.OrderByDescending(e => e.CreatedAt);
        }

        return events;
    }

    private long GetNewId()
    {
        return this.Select(evt => evt.Id).DefaultIfEmpty().Max() + 1;
    }
}
