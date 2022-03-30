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
        [JsonPropertyName("id")]
        public int Id;

        /// <summary>
        /// Example john_doe
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName;

        /// <summary>
        /// Example John Doe
        /// </summary>
        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("avatar_url")]
        public string AvatarURL;

        /// <summary>
        /// Example active
        /// </summary>
        [JsonPropertyName("state")]
        public string State;

        /// <summary>
        /// Membership creation date
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        /// <summary>
        /// Should be a value within <see cref="Models.AccessLevel"/>
        /// </summary>
        [JsonPropertyName("access_level")]
        public int AccessLevel;

        /// <summary>
        /// Membership expiration date
        /// </summary>
        [JsonPropertyName("expires_at")]
        public DateTime? ExpiresAt;
    }
}
