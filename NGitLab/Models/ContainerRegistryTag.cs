using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ContainerRegistryTag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("revision")]
    public string Revision { get; set; }

    [JsonPropertyName("short_revision")]
    public string ShortRevision { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("total_size")]
    public long TotalSize { get; set; }
}
