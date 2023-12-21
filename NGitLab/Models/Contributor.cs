using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Contributor
{
    public const string Url = "/contributors";

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("email")]
    public string Email;

    [JsonPropertyName("commits")]
    public int Commits;

    [JsonPropertyName("additions")]
    public int Addition;

    [JsonPropertyName("deletions")]
    public int Deletions;
}
