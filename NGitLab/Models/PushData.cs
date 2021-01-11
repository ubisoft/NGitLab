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
    }
}
