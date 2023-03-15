using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class CommitStatusCreate
    {
        [Obsolete("Unnecessary in the POSTed JSON; the 'Project Id' is actually specified through the endpoint URL.")]
        [JsonIgnore]
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
