using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitStatusCreate
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int ProjectId;

        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string CommitSha;

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public string Status;

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "target_url")]
        [JsonPropertyName("target_url")]
        public string TargetUrl;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "coverage")]
        [JsonPropertyName("coverage")]
        public int? Coverage;

        [DataMember(Name = "pipeline_id")]
        [JsonPropertyName("pipeline_id")]
        public int? PipelineId;
    }
}
