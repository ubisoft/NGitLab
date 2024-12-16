using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Membership
{
    /// <summary>
    /// Example: 3
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Example john_doe
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    /// <summary>
    /// Example John Doe
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarURL { get; set; }

    /// <summary>
    /// Example active
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; }

    /// <summary>
    /// Membership creation date
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Should be a value within <see cref="Models.AccessLevel"/>
    /// </summary>
    [JsonPropertyName("access_level")]
    public int AccessLevel { get; set; }

    /// <summary>
    /// Membership expiration date
    /// </summary>
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}
