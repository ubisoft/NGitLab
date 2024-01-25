using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Range
{
    [JsonPropertyName("line_code")]
    public string LineCode { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
