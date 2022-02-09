using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class AccessLevelInfo
    {
        [DataMember(Name = "access_level")]
        [JsonPropertyName("access_level")]
        public AccessLevel AccessLevel { get; set; }

        [DataMember(Name = "access_level_description")]
        [JsonPropertyName("access_level_description")]
        public string Description { get; set; }
    }
}
