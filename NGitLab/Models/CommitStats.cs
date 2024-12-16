using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CommitStats
{
    [JsonPropertyName("additions")]
    public int Additions { get; set; }

    [JsonPropertyName("deletions")]
    public int Deletions { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}
