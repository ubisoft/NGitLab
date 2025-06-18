using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProjectLabelCreate
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("color")]
    public required string Color { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
