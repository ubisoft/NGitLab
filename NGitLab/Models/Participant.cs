using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Participant
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarURL { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }
}
