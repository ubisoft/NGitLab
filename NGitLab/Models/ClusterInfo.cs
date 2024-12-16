using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ClusterInfo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platform_type")]
    public string PlatformType { get; set; }

    [JsonPropertyName("environment_scope")]
    public string EnvionmentScope { get; set; }
}
