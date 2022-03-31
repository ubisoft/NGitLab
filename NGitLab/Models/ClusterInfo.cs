using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class ClusterInfo
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("platform_type")]
        public string PlatformType;

        [JsonPropertyName("environment_scope")]
        public string EnvionmentScope;
    }
}
