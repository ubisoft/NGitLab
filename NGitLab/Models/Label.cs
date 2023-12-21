using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Label
{
    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("color")]
    public string Color;

    [JsonPropertyName("description")]
    public string Description;
}
