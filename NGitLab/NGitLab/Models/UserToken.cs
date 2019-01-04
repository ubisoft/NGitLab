using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class UserToken
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "revoked")]
        public bool Revoked { get; set; }

        [DataMember(Name = "scopes")]
        public string[] Scopes { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "active")]
        public bool Active { get; set; }

        [DataMember(Name = "impersonation")]
        public bool Impersonation { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

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
    }
}
