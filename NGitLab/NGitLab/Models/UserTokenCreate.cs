using System;
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
                if (!string.IsNullOrEmpty(ExpiresAtStr))
                {
                    return DateTime.Parse(ExpiresAtStr);
                }
                return null;
            }
            set { ExpiresAtStr = value.HasValue ? value.Value.ToString("yyyy-MM-dd") : null; }
        }

        [DataMember(Name = "scopes")]
        public string[] Scopes { get; set; }
    }
}
