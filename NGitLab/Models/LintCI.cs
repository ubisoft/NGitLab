using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class LintCI
{
    public const string Url = "/ci/lint";

    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    [JsonPropertyName("merged_yaml")]
    public string MergedYaml { get; set; }

    [JsonPropertyName("errors")]
    public string[] Errors { get; set; }

    [JsonPropertyName("warnings")]
    public string[] Warnings { get; set; }
}
