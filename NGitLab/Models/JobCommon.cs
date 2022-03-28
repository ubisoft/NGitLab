using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class JobCommon
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref;

        [DataMember(Name = "commit")]
        [JsonPropertyName("commit")]
        public Commit Commit;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "started_at")]
        [JsonPropertyName("started_at")]
        public DateTime StartedAt;

        [DataMember(Name = "finished_at")]
        [JsonPropertyName("finished_at")]
        public DateTime FinishedAt;

        [DataMember(Name = "stage")]
        [JsonPropertyName("stage")]
        public string Stage;

        [DataMember(Name = "coverage")]
        [JsonPropertyName("coverage")]
        public double? Coverage;

        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public JobStatus Status;

        [DataMember(Name = "tag")]
        [JsonPropertyName("tag")]
        public bool Tag;

        [DataMember(Name = "allow_failure")]
        [JsonPropertyName("allow_failure")]
        public bool AllowFailure;

        [DataMember(Name = "user")]
        [JsonPropertyName("user")]
        public User User;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;

        [DataMember(Name = "duration")]
        [JsonPropertyName("duration")]
        public float? Duration;

        [DataMember(Name = "queued_duration")]
        [JsonPropertyName("queued_duration")]
        public float? QueuedDuration;

        [DataContract]
        public class JobPipeline
        {
            [DataMember(Name = "id")]
            [JsonPropertyName("id")]
            public int Id;

            [DataMember(Name = "ref")]
            [JsonPropertyName("ref")]
            public string Ref;

            [DataMember(Name = "sha")]
            [JsonPropertyName("sha")]
            public Sha1 Sha;

            [DataMember(Name = "status")]
            [JsonPropertyName("status")]
            public JobStatus Status;
        }
    }
}
