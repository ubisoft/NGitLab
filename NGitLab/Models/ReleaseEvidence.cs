using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseEvidence
    {
        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string Sha { get; set; }

        [DataMember(Name = "filepath")]
        [JsonPropertyName("filepath")]
        public string Filepath { get; set; }

        [DataMember(Name = "collected_at")]
        [JsonPropertyName("collected_at")]
        public DateTime CollectedAt { get; set; }
    }
}
