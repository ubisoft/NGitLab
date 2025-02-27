using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Label
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("open_issues_count")]
    public int OpenIssuesCount { get; set; }

    [JsonPropertyName("closed_issues_count")]
    public int ClosedIssuesCount { get; set; }

    [JsonPropertyName("open_merge_requests_count")]
    public int OpenMergeRequestsCount { get; set; }
}
