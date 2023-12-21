using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CompareResults
{
    [JsonPropertyName("commit")]
    public Commit Commit { get; set; }

    [JsonPropertyName("commits")]
    public Commit[] Commits { get; set; }

    [JsonPropertyName("diffs")]
    public Diff[] Diff { get; set; }

    [JsonPropertyName("compare_timeout")]
    public bool CompareTimeout { get; set; }

    [JsonPropertyName("compare_same_ref")]
    public bool CompareSameRefs { get; set; }
}
