using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class AccessLevelInfo
    {
        [DataMember(Name = "access_level")]
        public AccessLevel AccessLevel { get; set; }

        [DataMember(Name = "access_level_description")]
        public string Description { get; set; }
    }
}
