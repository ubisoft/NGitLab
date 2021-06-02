using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PushData
    {
        [DataMember(Name = "commit_count")]
        public int CommitCount { get; set; }

        [DataMember(Name = "action")]
        public PushDataAction Action { get; set; }

        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "ref_type")]
        public CommitRefType RefType { get; set; }

        [DataMember(Name = "commit_title")]
        public string CommitTitle { get; set; }
    }
}
