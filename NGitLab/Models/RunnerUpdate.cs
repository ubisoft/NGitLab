using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerUpdate
    {
        [DataMember(Name = "active")]
        [JsonPropertyName("active")]
        public bool? Active;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "locked")]
        [JsonPropertyName("locked")]
        public bool? Locked;

        [DataMember(Name = "run_untagged")]
        [JsonPropertyName("run_untagged")]
        public bool? RunUntagged;

        [DataMember(Name = "tag_list")]
        [JsonPropertyName("tag_list")]
        public string[] TagList;
    }
}
