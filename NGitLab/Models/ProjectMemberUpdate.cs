using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectMemberUpdate
    {
        [DataMember(Name = "user_id")]
        public string UserId;

        [DataMember(Name = "access_level")]
        public AccessLevel AccessLevel;

        [DataMember(Name = "expires_at")]
        public string ExpiresAt;
    }
}
