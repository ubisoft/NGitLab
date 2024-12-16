using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class UserTokenCreate
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    [JsonPropertyName("scopes")]
    public string[] Scopes { get; set; }
}
