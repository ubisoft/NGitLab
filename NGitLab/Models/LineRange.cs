using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class LineRange
{
    [JsonPropertyName("start")]
    public Range Start { get; set; }

    [JsonPropertyName("end")]
    public Range End { get; set; }
}
