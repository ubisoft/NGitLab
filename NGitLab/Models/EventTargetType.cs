using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public enum EventTargetType
    {
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
    }
}
