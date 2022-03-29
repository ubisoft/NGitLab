using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class JobBasic
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        [DataMember(Name = "commit")]
        [JsonPropertyName("commit")]
        public Commit Commit { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "started_at")]
        [JsonPropertyName("started_at")]
        public DateTime StartedAt { get; set; }

        [DataMember(Name = "finished_at")]
        [JsonPropertyName("finished_at")]
        public DateTime FinishedAt { get; set; }

        [DataMember(Name = "stage")]
        [JsonPropertyName("stage")]
        public string Stage { get; set; }

        [DataMember(Name = "coverage")]
        [JsonPropertyName("coverage")]
        public double? Coverage { get; set; }

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public JobStatus Status { get; set; }

        [DataMember(Name = "tag")]
        [JsonPropertyName("tag")]
        public bool Tag { get; set; }

        [DataMember(Name = "allow_failure")]
        [JsonPropertyName("allow_failure")]
        public bool AllowFailure { get; set; }

        [DataMember(Name = "user")]
        [JsonPropertyName("user")]
        public User User { get; set; }

        [DataMember(Name = "pipeline")]
        [JsonPropertyName("pipeline")]
        public JobPipeline Pipeline { get; set; }

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl { get; set; }

        [DataMember(Name = "duration")]
        [JsonPropertyName("duration")]
        public float? Duration { get; set; }

        [DataMember(Name = "queued_duration")]
        [JsonPropertyName("queued_duration")]
        public float? QueuedDuration { get; set; }

        [DataContract]
        public class JobPipeline
        {
            [DataMember(Name = "id")]
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [DataMember(Name = "ref")]
            [JsonPropertyName("ref")]
            public string Ref { get; set; }

            [DataMember(Name = "sha")]
            [JsonPropertyName("sha")]
            public Sha1 Sha { get; set; }

            [DataMember(Name = "status")]
            [JsonPropertyName("status")]
            public JobStatus Status { get; set; }
        }
    }
}
