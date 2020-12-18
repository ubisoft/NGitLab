using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussionResolve
    {
        [DataMember(Name = "discussion_id")]
        public string Id;

        [DataMember(Name = "resolved")]
        public bool Resolved;
    }
}
