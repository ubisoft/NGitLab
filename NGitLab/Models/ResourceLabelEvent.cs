using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ResourceLabelEvent
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "user")]
        [JsonPropertyName("user")]
        public Author User { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "resource_id")]
        [JsonPropertyName("resource_id")]
        public int ResourceId { get; set; }

        [DataMember(Name = "label")]
        [JsonPropertyName("label")]
        public Label Label { get; set; }

        [DataMember(Name = "action")]
        [JsonPropertyName("action")]
        public ResourceLabelEventAction Action { get; set; }
    }
}
