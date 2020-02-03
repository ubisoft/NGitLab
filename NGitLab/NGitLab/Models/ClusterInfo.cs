using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ClusterInfo
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "platform_type")]
        public string PlatformType;

        [DataMember(Name = "environment_scope")]
        public string EnvionmentScope;
    }
}
