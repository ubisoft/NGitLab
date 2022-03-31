using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class EnvironmentInfo
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("slug")]
        public string Slug;

        [JsonPropertyName("external_url")]
        public string ExternalUrl;

        [JsonPropertyName("state")]
        public string State;
    }
}
