using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class UserToken
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("revoked")]
    public bool Revoked { get; set; }

    [JsonPropertyName("scopes")]
    public string[] Scopes { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("impersonation")]
    public bool Impersonation { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}
