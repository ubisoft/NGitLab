using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Author
{
    [JsonPropertyName("id")]
    public long Id;

    [JsonPropertyName("username")]
    public string Username;

    [JsonPropertyName("email")]
    public string Email;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("state")]
    public string State;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl;

    [JsonPropertyName("web_url")]
    public string WebUrl;
}
