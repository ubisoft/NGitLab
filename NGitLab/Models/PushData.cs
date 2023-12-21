using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PushData
{
    [JsonPropertyName("commit_count")]
    public int CommitCount { get; set; }

    [JsonPropertyName("action")]
    public PushDataAction Action { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("ref_type")]
    public CommitRefType RefType { get; set; }

    [JsonPropertyName("commit_title")]
    public string CommitTitle { get; set; }
}
