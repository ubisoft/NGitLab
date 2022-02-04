using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PushData
    {
        [DataMember(Name = "commit_count")]
        [JsonPropertyName("commit_count")]
        public int CommitCount { get; set; }

        [DataMember(Name = "action")]
        [JsonPropertyName("action")]
        public PushDataAction Action { get; set; }

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        [DataMember(Name = "ref_type")]
        [JsonPropertyName("ref_type")]
        public CommitRefType RefType { get; set; }

        [DataMember(Name = "commit_title")]
        [JsonPropertyName("commit_title")]
        public string CommitTitle { get; set; }
    }
}
