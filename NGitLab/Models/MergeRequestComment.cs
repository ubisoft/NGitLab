using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class MergeRequestComment
    {
        [JsonPropertyName("id")]
        public long Id;

        [JsonPropertyName("body")]
        public string Body;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [JsonPropertyName("author")]
        public User Author { get; set; }

        [JsonPropertyName("resolved")]
        public bool Resolved;

        [JsonPropertyName("resolvable")]
        public bool Resolvable;

        [JsonPropertyName("type")]
        public string Type;

        [JsonPropertyName("system")]
        public bool System;
    }
}
