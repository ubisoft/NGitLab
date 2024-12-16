using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Diff
{
    [JsonPropertyName("diff")]
    public string Difference { get; set; }

    [JsonPropertyName("new_path")]
    public string NewPath { get; set; }

    [JsonPropertyName("old_path")]
    public string OldPath { get; set; }

    [JsonPropertyName("a_mode")]
    public string AMode { get; set; }

    [JsonPropertyName("b_mode")]
    public string BMode { get; set; }

    [JsonPropertyName("new_file")]
    public bool IsNewFile { get; set; }

    [JsonPropertyName("renamed_file")]
    public bool IsRenamedFile { get; set; }

    [JsonPropertyName("deleted_file")]
    public bool IsDeletedFile { get; set; }
}
