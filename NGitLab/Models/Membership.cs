using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Membership
    {
        /// <summary>
        /// Example: 3
        /// </summary>
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        /// <summary>
        /// Example john_doe
        /// </summary>
        [DataMember(Name = "username")]
        [JsonPropertyName("username")]
        public string UserName;

        /// <summary>
        /// Example John Doe
        /// </summary>
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "avatar_url")]
        [JsonPropertyName("avatar_url")]
        public string AvatarURL;

        /// <summary>
        /// Example active
        /// </summary>
        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;

        /// <summary>
        /// Membership creation date
        /// </summary>
        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        /// <summary>
        /// Should be a value within <see cref="Models.AccessLevel"/>
        /// </summary>
        [DataMember(Name = "access_level")]
        [JsonPropertyName("access_level")]
        public int AccessLevel;

        /// <summary>
        /// Membership expiration date
        /// </summary>
        [DataMember(Name = "expires_at")]
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt;
    }
}
