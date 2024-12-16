using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Range
{
    [JsonPropertyName("line_code")]
    public string LineCode { get; set; }

    [JsonPropertyName("type")]
    public DynamicEnum<RangeType>? Type { get; set; }
}
