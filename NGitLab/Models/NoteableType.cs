using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public enum NoteableType
    {
        None = 0,
        Issue,
        [EnumMember(Value = "merge_request")]
        MergeRequest,
        Snippet,
        Commit,
        Epic,
    }
}
