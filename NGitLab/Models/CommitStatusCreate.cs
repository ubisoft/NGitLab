using System.Text.Json.Serialization;

namespace NGitLab.Models;

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
