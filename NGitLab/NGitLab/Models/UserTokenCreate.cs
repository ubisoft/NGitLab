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

        public DateTime ExpiresAt
        {
            get { return !string.IsNullOrEmpty(ExpiresAtStr) ? DateTime.Parse(ExpiresAtStr) : DateTime.MinValue; }
            set { ExpiresAtStr = value.ToString("yyyy-MM-dd"); }
        }

        [DataMember(Name = "scopes")]
        public string[] Scopes { get; set; }
    }
}
