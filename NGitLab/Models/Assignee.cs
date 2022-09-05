using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Assignee
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("username")]
        public string Username;

        [JsonPropertyName("email")]
        public string Email;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("state")]
        public string State;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;
        
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl;
    }
}
