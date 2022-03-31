using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class GroupMemberUpdate
    {
        [JsonPropertyName("user_id")]
        public string UserId;

        [JsonPropertyName("access_level")]
        public AccessLevel AccessLevel;

        [JsonPropertyName("expires_at")]
        public string ExpiresAt;
    }
}
