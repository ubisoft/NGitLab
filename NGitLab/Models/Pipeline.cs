using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Pipeline
{
    public const string Url = "/pipelines";

    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("status")]
    public JobStatus Status;

    [JsonPropertyName("ref")]
    public string Ref;

    [JsonPropertyName("tag")]
    public bool Tag;

    [JsonPropertyName("sha")]
    public Sha1 Sha;

    [JsonPropertyName("before_sha")]
    public Sha1 BeforeSha;

    [JsonPropertyName("yaml_errors")]
    public string YamlError;

    [JsonPropertyName("user")]
    public User User;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt;

    [JsonPropertyName("started_at")]
    public DateTime StartedAt;

    [JsonPropertyName("finished_at")]
    public DateTime FinishedAt;

    [JsonPropertyName("committed_at")]
    public DateTime CommittedAt;

    /// <summary>
    /// Duration in seconds.
    /// </summary>
    [JsonPropertyName("duration")]
    public long? Duration;

    [JsonPropertyName("coverage")]
    public double Coverage;

    [JsonPropertyName("web_url")]
    public string WebUrl;

    [JsonPropertyName("detailed_status")]
    public PipelineDetailedStatus DetailedStatus { get; set; }

    [JsonPropertyName("source")]
    public string Source;

    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }
}
