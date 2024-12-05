using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class References
{
    [JsonPropertyName("short")]
    public string Short { get; set; }

    [JsonPropertyName("relative")]
    public string Relative { get; set; }

    [JsonPropertyName("full")]
    public string Full { get; set; }
}
