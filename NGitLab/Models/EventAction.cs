using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum EventAction
{
    Accepted,
    [EnumMember(Value = "approved")]
    Approved,
    [EnumMember(Value = "created")]
    Created,
    [EnumMember(Value = "updated")]
    Updated,
    Uploaded,
    Deleted,
    [EnumMember(Value = "closed")]
    Closed,
    Opened,
    [EnumMember(Value = "reopened")]
    Reopened,
    [EnumMember(Value = "pushed")]
    Pushed,
    [EnumMember(Value = "commented")]
    Commented,
    [EnumMember(Value = "merged")]
    Merged,
    [EnumMember(Value = "joined")]
    Joined,
    [EnumMember(Value = "left")]
    Left,
    [EnumMember(Value = "destroyed")]
    Destroyed,
    [EnumMember(Value = "expired")]
    Expired,
    [EnumMember(Value = "pushed new")]
    PushedNew,
    [EnumMember(Value = "pushed to")]
    PushedTo,
    [EnumMember(Value = "commented on")]
    CommentedOn,
}
