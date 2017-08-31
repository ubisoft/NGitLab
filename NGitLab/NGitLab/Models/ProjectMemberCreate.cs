using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectMemberCreate
    {
        [DataMember(Name = "user_id")]
        public string UserId;

        [DataMember(Name = "access_level")]
        public AccessLevel AccessLevel;
    }
}
