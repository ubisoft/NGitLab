using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Pipeline
    {
        public const string Url = "/pipelines";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public JobStatus Status;

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref;

        [DataMember(Name = "tag")]
        [JsonPropertyName("tag")]
        public bool Tag;

        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public Sha1 Sha;

        [DataMember(Name = "before_sha")]
        [JsonPropertyName("before_sha")]
        public Sha1 BeforeSha;

        [DataMember(Name = "yaml_errors")]
        [JsonPropertyName("yaml_errors")]
        public string YamlError;

        [DataMember(Name = "user")]
        [JsonPropertyName("user")]
        public User User;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "started_at")]
        [JsonPropertyName("started_at")]
        public DateTime StartedAt;

        [DataMember(Name = "finished_at")]
        [JsonPropertyName("finished_at")]
        public DateTime FinishedAt;

        [DataMember(Name = "committed_at")]
        [JsonPropertyName("committed_at")]
        public DateTime CommittedAt;

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        [DataMember(Name = "duration")]
        [JsonPropertyName("duration")]
        public long? Duration;

        [DataMember(Name = "coverage")]
        [JsonPropertyName("coverage")]
        public double Coverage;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;

        [DataMember(Name = "detailed_status")]
        [JsonPropertyName("detailed_status")]
        public PipelineDetailedStatus DetailedStatus { get; set; }
    }
}
