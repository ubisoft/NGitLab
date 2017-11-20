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

        /// <summary>
        /// format: yyyy-MM-dd
        /// </summary>
        [DataMember(Name = "expires_at")]
        public string ExpiresAt { get; set; }

        [DataMember(Name = "scopes")]
        public string[] Scopes { get; set; }
    }
}
