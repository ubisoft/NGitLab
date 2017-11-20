using System;
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
        public string ExpiresAt { get; set; }
    }
}
