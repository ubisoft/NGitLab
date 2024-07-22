using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Participant
{
    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("username")]
    public string Username;

    [JsonPropertyName("state")]
    public string State;

    [JsonPropertyName("avatar_url")]
    public string AvatarURL;

    [JsonPropertyName("web_url")]
    public string WebUrl;
}
