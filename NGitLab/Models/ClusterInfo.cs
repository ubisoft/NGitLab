using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ClusterInfo
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "platform_type")]
        [JsonPropertyName("platform_type")]
        public string PlatformType;

        [DataMember(Name = "environment_scope")]
        [JsonPropertyName("environment_scope")]
        public string EnvionmentScope;
    }
}
