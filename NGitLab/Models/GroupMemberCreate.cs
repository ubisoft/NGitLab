using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class GroupMemberCreate
{
    /// <summary>
    /// The Id of the user. Must be an integer value.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("access_level")]
    public AccessLevel AccessLevel { get; set; }

    /// <summary>
    /// The optional expiration date. Must be null or a value like "yyyy-MM-dd".
    /// </summary>
    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; }
}
