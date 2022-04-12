using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Ref
    {
        [JsonPropertyName("type")]
        public string Type;

        [JsonPropertyName("name")]
        public string Name;
    }
}
