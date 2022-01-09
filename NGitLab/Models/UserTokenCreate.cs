using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserTokenCreate
    {
        [DataMember(Name = "user_id")]
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonInclude]
        [DataMember(Name = "expires_at")]
        [JsonPropertyName("expires_at")]
        public string ExpiresAtStr { get; private set; }

        [JsonIgnore]
        public DateTime? ExpiresAt
        {
            get
            {
                if (!string.IsNullOrEmpty(ExpiresAtStr) &&
                    DateTime.TryParseExact(ExpiresAtStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expiresAt))
                {
                    return expiresAt;
                }

                return null;
            }
            set => ExpiresAtStr = value?.ToString("yyyy-MM-dd");
        }

        [DataMember(Name = "scopes")]
        [JsonPropertyName("scopes")]
        public string[] Scopes { get; set; }
    }
}
