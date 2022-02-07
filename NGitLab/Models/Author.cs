using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Author
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "username")]
        [JsonPropertyName("username")]
        public string Username;

        [DataMember(Name = "email")]
        [JsonPropertyName("email")]
        public string Email;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "avatar_url")]
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;
    }
}
