using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CommitStatus
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("sha")]
    public string CommitSha { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("target_url")]
    public string TargetUrl { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("coverage")]
    public int? Coverage { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }
}
