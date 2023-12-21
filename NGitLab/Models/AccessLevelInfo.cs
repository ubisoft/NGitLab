using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class AccessLevelInfo
{
    [JsonPropertyName("access_level")]
    public AccessLevel AccessLevel { get; set; }

    [JsonPropertyName("access_level_description")]
    public string Description { get; set; }
}
