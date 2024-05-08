using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum EventTargetType
{
    None = 0,
    Issue,
    Milestone,
    [EnumMember(Value = "merge_request")]
    MergeRequest,
    Note,
    Project,
    Snippet,
    User,
    DiffNote,
    DiscussionNote,
    [EnumMember(Value = "DesignManagement::Design")]
    DesignManagement,
    WorkItem,
}
