using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestRebase
{
    /// <summary>
    /// Set to true to skip creating a CI pipeline.
    /// </summary>
    [JsonPropertyName("skip_ci")]
    public bool? SkipCi { get; set; }
}
