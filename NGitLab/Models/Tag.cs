using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Tag
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("commit")]
    public CommitInfo Commit { get; set; }

    [JsonPropertyName("release")]
    public ReleaseInfo Release { get; set; }

    [JsonPropertyName("target")]
    public Sha1 Target { get; set; }

    [JsonPropertyName("protected")]
    public bool Protected { get; set; }
}
