using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class GroupLabelCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
