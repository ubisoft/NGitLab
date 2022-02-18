using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class GroupMemberCreate
    {
        [DataMember(Name = "user_id")]
        [JsonPropertyName("user_id")]
        public string UserId;

        [DataMember(Name = "access_level")]
        [JsonPropertyName("access_level")]
        public AccessLevel AccessLevel;

        [DataMember(Name = "expires_at")]
        [JsonPropertyName("expires_at")]
        public string ExpiresAt;
    }
}
