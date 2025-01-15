using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProjectMergeRequestTemplate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}
