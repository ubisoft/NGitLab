using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerRegister
    {
        [DataMember(Name = "token")]
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [DataMember(Name = "active")]
        [JsonPropertyName("active")]
        public bool? Active { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "locked")]
        [JsonPropertyName("locked")]
        public bool? Locked { get; set; }

        [DataMember(Name = "run_untagged")]
        [JsonPropertyName("run_untagged")]
        public bool? RunUntagged { get; set; }

        [DataMember(Name = "tag_list")]
        [JsonPropertyName("tag_list")]
        public string[] TagList { get; set; }
    }
}
