using System;
using System.Globalization;

namespace NGitLab.Mock;

public sealed class Milestone : GitLabObject
{
    public Project Project => Parent as Project;

    public Group Group => Parent as Group;

    public long Id { get; set; }

    public long Iid { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? ClosedAt { get; set; }

    public MilestoneState State
    {
        get
        {
            if (ClosedAt.HasValue)
                return MilestoneState.closed;

            return MilestoneState.active;
        }

        set
        {
            if (value == MilestoneState.closed)
            {
                ClosedAt = DateTimeOffset.UtcNow;
            }
            else if (value == MilestoneState.active)
            {
                ClosedAt = null;
            }
        }
    }

    public Milestone()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public Models.Milestone ToClientMilestone()
    {
        return new Models.Milestone
        {
            Id = Id,
            Title = Title,
            Description = Description,
            DueDate = DueDate.UtcDateTime.ToString(CultureInfo.InvariantCulture),
            StartDate = StartDate.UtcDateTime.ToString(CultureInfo.InvariantCulture),
            State = State.ToString(),
            CreatedAt = CreatedAt.UtcDateTime,
            UpdatedAt = UpdatedAt.UtcDateTime,
        };
    }
}
