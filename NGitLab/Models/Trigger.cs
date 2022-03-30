using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Trigger
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("last_used")]
        public DateTime LastUsed;

        [JsonPropertyName("token")]
        public string Token;

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;
    }
}
