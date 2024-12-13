using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ResourceLabelEvent
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("user")]
    public Author User { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("resource_id")]
    public long ResourceId { get; set; }

    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; }

    [JsonPropertyName("label")]
    public Label Label { get; set; }

    [JsonPropertyName("action")]
    public ResourceLabelEventAction Action { get; set; }
}
