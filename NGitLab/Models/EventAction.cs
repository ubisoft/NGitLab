using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public enum EventAction
    {
        Accepted,
        Approved,
        Created,
        Updated,
        Uploaded,
        Deleted,
        Closed,
        Opened,
        Reopened,
        Pushed,
        Commented,
        Merged,
        Joined,
        Left,
        Destroyed,
        Expired,
        [EnumMember(Value = "pushed new")]
        PushedNew,
        [EnumMember(Value = "pushed to")]
        PushedTo,
        [EnumMember(Value = "commented on")]
        CommentedOn,
    }
}
