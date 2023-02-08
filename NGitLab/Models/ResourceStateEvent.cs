using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class ResourceStateEvent
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user")]
        public Author User { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("resource_id")]
        public int ResourceId { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
