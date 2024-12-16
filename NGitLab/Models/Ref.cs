using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Ref
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
