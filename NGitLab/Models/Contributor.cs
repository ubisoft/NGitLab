using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Contributor
{
    public const string Url = "/contributors";

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("commits")]
    public int Commits { get; set; }

    [JsonPropertyName("additions")]
    public int Addition { get; set; }

    [JsonPropertyName("deletions")]
    public int Deletions { get; set; }
}
