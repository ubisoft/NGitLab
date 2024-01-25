using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Position
{
    [JsonPropertyName("new_path")]
    public string NewPath { get; set; }

    [JsonPropertyName("old_line")]
    public int OldLine { get; set; }

    [JsonPropertyName("new_line")]
    public int NewLine { get; set; }

    [JsonPropertyName("position_type")]
    public string PositionType { get; set; }

    [JsonPropertyName("line_range")]
    public LineRange LineRange { get; set; }
}
