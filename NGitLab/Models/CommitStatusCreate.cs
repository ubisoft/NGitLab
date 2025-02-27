using System.Text.Json.Serialization;

namespace NGitLab.Models;

/// <summary>
/// JSON properties used to add or update a pipeline status of a commit. Refer to
/// <see href="https://docs.gitlab.com/api/commits/#set-the-pipeline-status-of-a-commit">GitLab's documentation</see>
/// for further details.
/// </summary>
public class CommitStatusCreate
{
    [JsonPropertyName("sha")]
    public string CommitSha { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("target_url")]
    public string TargetUrl { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("coverage")]
    public int? Coverage { get; set; }

    [JsonPropertyName("pipeline_id")]
    public long? PipelineId { get; set; }
}
