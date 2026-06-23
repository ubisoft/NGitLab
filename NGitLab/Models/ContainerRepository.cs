using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ContainerRepository
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("project_id")]
    public long ProjectId { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("tags_count")]
    public int TagsCount { get; set; }
}
