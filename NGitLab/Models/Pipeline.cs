using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Pipeline
{
    public const string Url = "/pipelines";

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("status")]
    public JobStatus Status { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("tag")]
    public bool Tag { get; set; }

    [JsonPropertyName("sha")]
    public Sha1 Sha { get; set; }

    [JsonPropertyName("before_sha")]
    public Sha1 BeforeSha { get; set; }

    [JsonPropertyName("yaml_errors")]
    public string YamlError { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("started_at")]
    public DateTime StartedAt { get; set; }

    [JsonPropertyName("finished_at")]
    public DateTime FinishedAt { get; set; }

    [JsonPropertyName("committed_at")]
    public DateTime CommittedAt { get; set; }

    /// <summary>
    /// Duration in seconds.
    /// </summary>
    [JsonPropertyName("duration")]
    public long? Duration { get; set; }

    [JsonPropertyName("coverage")]
    public double Coverage { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("detailed_status")]
    public PipelineDetailedStatus DetailedStatus { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("project_id")]
    public long ProjectId { get; set; }
}
