using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Position
{
    [JsonPropertyName("old_path")]
    public string OldPath { get; set; }

    [JsonPropertyName("new_path")]
    public string NewPath { get; set; }

    [JsonPropertyName("position_type")]
    public DynamicEnum<PositionType> PositionType { get; set; }

    [JsonPropertyName("old_line")]
    public int? OldLine { get; set; }

    [JsonPropertyName("new_line")]
    public int? NewLine { get; set; }

    [JsonPropertyName("line_range")]
    public LineRange LineRange { get; set; }

    [JsonPropertyName("base_sha")]
    public Sha1? BaseSha { get; set; }

    [JsonPropertyName("head_sha")]
    public Sha1 HeadSha { get; set; }

    [JsonPropertyName("start_sha")]
    public Sha1 StartSha { get; set; }
}
