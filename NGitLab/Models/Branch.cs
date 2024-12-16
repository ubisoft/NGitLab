using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Branch
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("merged")]
    public bool Merged { get; set; }

    [JsonPropertyName("protected")]
    public bool Protected { get; set; }

    [JsonPropertyName("default")]
    public bool Default { get; set; }

    [JsonPropertyName("developers_can_push")]
    public bool DevelopersCanPush { get; set; }

    [JsonPropertyName("developers_can_merge")]
    public bool DevelopersCanMerge { get; set; }

    [JsonPropertyName("can_push")]
    public bool CanPush { get; set; }

    [JsonPropertyName("commit")]
    public CommitInfo Commit { get; set; }
}
