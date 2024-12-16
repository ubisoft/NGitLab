using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectMemberUpdate
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("access_level")]
    public AccessLevel AccessLevel { get; set; }

    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; }
}
