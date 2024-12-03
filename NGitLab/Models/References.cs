using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class References
{
    [JsonPropertyName("short")]
    public string Short;

    [JsonPropertyName("relative")]
    public string Relative;

    [JsonPropertyName("full")]
    public string Full;
}
