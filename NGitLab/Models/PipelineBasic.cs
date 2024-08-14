using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineBasic
{
    public const string Url = "/pipelines";

    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("project_id")]
    public int ProjectId;

    [JsonPropertyName("status")]
    public JobStatus Status;

    [JsonPropertyName("ref")]
    public string Ref;

    [JsonPropertyName("sha")]
    public Sha1 Sha;

    [JsonPropertyName("source")]
    public string Source;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt;

    [JsonPropertyName("web_url")]
    public string WebUrl;

    [JsonPropertyName("name")]
    public string Name;
}
