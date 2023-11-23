using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class AccessLevelInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("access_level")]
        public AccessLevel AccessLevel { get; set; }

        [JsonPropertyName("access_level_description")]
        public string Description { get; set; }
    }
}
