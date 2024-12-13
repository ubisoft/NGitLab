using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CommitStatus
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("sha")]
    public string CommitSha;

    [JsonPropertyName("ref")]
    public string Ref;

    [JsonPropertyName("status")]
    public string Status;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("target_url")]
    public string TargetUrl;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("coverage")]
    public int? Coverage;

    [JsonPropertyName("author")]
    public Author Author;
}
