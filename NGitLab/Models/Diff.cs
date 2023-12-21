using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Diff
{
    [JsonPropertyName("diff")]
    public string Difference;

    [JsonPropertyName("new_path")]
    public string NewPath;

    [JsonPropertyName("old_path")]
    public string OldPath;

    [JsonPropertyName("a_mode")]
    public string AMode;

    [JsonPropertyName("b_mode")]
    public string BMode;

    [JsonPropertyName("new_file")]
    public bool IsNewFile;

    [JsonPropertyName("renamed_file")]
    public bool IsRenamedFile;

    [JsonPropertyName("deleted_file")]
    public bool IsDeletedFile;
}
