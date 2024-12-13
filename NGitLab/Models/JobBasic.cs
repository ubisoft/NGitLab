using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class JobBasic
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("commit")]
    public Commit Commit { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("started_at")]
    public DateTime StartedAt { get; set; }

    [JsonPropertyName("finished_at")]
    public DateTime FinishedAt { get; set; }

    [JsonPropertyName("stage")]
    public string Stage { get; set; }

    [JsonPropertyName("coverage")]
    public double? Coverage { get; set; }

    [JsonPropertyName("status")]
    public JobStatus Status { get; set; }

    [JsonPropertyName("tag")]
    public bool Tag { get; set; }

    [JsonPropertyName("allow_failure")]
    public bool AllowFailure { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("pipeline")]
    public JobPipeline Pipeline { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("duration")]
    public float? Duration { get; set; }

    [JsonPropertyName("queued_duration")]
    public float? QueuedDuration { get; set; }

    [JsonPropertyName("tag_list")]
    public string[] TagList { get; set; }

    [JsonPropertyName("failure_reason")]
    public string FailureReason { get; set; }

    public class JobPipeline
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("project_id")]
        public long ProjectId { get; set; }

        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        [JsonPropertyName("sha")]
        public Sha1 Sha { get; set; }

        [JsonPropertyName("status")]
        public JobStatus Status { get; set; }
    }
}
