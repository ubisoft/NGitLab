using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Snippet
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("file_name")]
        public string FileName;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("author")]
        public Author Author;

        [JsonPropertyName("updated_at")]
        public string UpdatedAt;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("web_url")]
        public string WebUrl;
    }
}
