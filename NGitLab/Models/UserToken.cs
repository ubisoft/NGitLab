using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserToken
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "revoked")]
        [JsonPropertyName("revoked")]
        public bool Revoked { get; set; }

        [DataMember(Name = "scopes")]
        [JsonPropertyName("scopes")]
        public string[] Scopes { get; set; }

        [DataMember(Name = "token")]
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [DataMember(Name = "active")]
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [DataMember(Name = "impersonation")]
        [JsonPropertyName("impersonation")]
        public bool Impersonation { get; set; }

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "expires_at")]
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}
