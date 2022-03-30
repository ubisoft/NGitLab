using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseEvidence
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; }

        [JsonPropertyName("filepath")]
        public string Filepath { get; set; }

        [JsonPropertyName("collected_at")]
        public DateTime CollectedAt { get; set; }
    }
}
