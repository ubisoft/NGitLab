using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class EnvironmentInfo
{
    [JsonPropertyName("id")]
    public long Id;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("slug")]
    public string Slug;

    [JsonPropertyName("external_url")]
    public string ExternalUrl;

    [JsonPropertyName("state")]
    public string State;

    [JsonPropertyName("last_deployment")]
    public EnvironmentLastDeployment LastDeployment;
}
