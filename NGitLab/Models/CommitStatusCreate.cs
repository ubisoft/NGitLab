using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStatusCreate
    {
        [JsonPropertyName("id")]
        public int ProjectId;

        [JsonPropertyName("sha")]
        public string CommitSha;

        [JsonPropertyName("state")]
        public string State;

        [JsonPropertyName("status")]
        public string Status;

        [JsonPropertyName("ref")]
        public string Ref;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("target_url")]
        public string TargetUrl;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("coverage")]
        public int? Coverage;

        [JsonPropertyName("pipeline_id")]
        public int? PipelineId;
    }
}
