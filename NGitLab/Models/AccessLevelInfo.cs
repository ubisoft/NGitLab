using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class AccessLevelInfo
    {
        [JsonPropertyName("access_level")]
        public AccessLevel AccessLevel { get; set; }

        [JsonPropertyName("access_level_description")]
        public string Description { get; set; }
    }
}
