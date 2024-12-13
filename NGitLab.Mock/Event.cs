using System;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class Event : GitLabObject
{
    public long Id { get; set; }

    public string Title { get; set; }

    public long ProjectId { get; set; }

    public DynamicEnum<EventAction> Action { get; set; }

    public long? TargetId { get; set; }

    public long? TargetIId { get; set; }

    public DynamicEnum<EventTargetType> TargetType { get; set; }

    public string TargetTitle { get; set; }

    public long AuthorId { get; set; }

    public string AuthorUserName { get; set; }

    public DateTime CreatedAt { get; set; }

    public Note Note { get; set; }

    public PushData PushData { get; set; }

    public Models.Event ToClientEvent()
    {
        return new Models.Event
        {
            Id = Id,
            Title = Title,
            ProjectId = ProjectId,
            Action = Action,
            TargetId = TargetId,
            TargetIId = TargetIId,
            TargetType = TargetType,
            TargetTitle = TargetTitle,
            AuthorId = AuthorId,
            AuthorUserName = AuthorUserName,
            CreatedAt = CreatedAt,
            Note = Note?.ToClientEvent(),
            PushData = PushData,
        };
    }
}
