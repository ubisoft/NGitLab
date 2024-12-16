using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Change
{
    [JsonPropertyName("old_path")]
    public string OldPath { get; set; }

    [JsonPropertyName("new_path")]
    public string NewPath { get; set; }

    [JsonPropertyName("a_mode")]
    public long AMode { get; set; }

    [JsonPropertyName("b_mode")]
    public long BMode { get; set; }

    [JsonPropertyName("new_file")]
    public bool NewFile { get; set; }

    [JsonPropertyName("renamed_file")]
    public bool RenamedFile { get; set; }

    [JsonPropertyName("deleted_file")]
    public bool DeletedFile { get; set; }

    [JsonPropertyName("diff")]
    public string Diff { get; set; }
}
