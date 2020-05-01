using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerUpdate
    {
        [DataMember(Name = "active")]
        public bool? Active;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "locked")]
        public bool? Locked;

        [DataMember(Name = "run_untagged")]
        public bool? RunUntagged;

        [DataMember(Name = "tag_list")]
        public string[] TagList;
    }
}