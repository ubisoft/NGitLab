using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerRegister
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "active")]
        public bool? Active { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "locked")]
        public bool? Locked { get; set; }

        [DataMember(Name = "run_untagged")]
        public bool? RunUntagged { get; set; }

        [DataMember(Name = "tag_list")]
        public string[] TagList { get; set; }
    }
}
