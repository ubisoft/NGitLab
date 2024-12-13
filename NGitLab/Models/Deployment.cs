using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Deployment
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("iid")]
    public long DeploymentId { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("environment")]
    public EnvironmentInfo Environment { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
