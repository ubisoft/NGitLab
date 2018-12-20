using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestNote : ProjectIssueNote
    {
        [DataMember(Name = "position")]
        public NotePosition Position;

        [DataMember(Name = "resolved")]
        public bool Resolved;
    }
}