using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserTokenCreate
    {
        [DataMember(Name = "user_id")]
        public int UserId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "expires_at")]
        private string ExpiresAtStr { get; set; }

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
        public string[] Scopes { get; set; }
    }
}
