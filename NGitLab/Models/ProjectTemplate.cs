using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProjectTemplate
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
