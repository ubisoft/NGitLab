using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProtectedTag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("create_access_levels")]
    public AccessLevelInfo[] CreateAccessLevels { get; set; }
}
