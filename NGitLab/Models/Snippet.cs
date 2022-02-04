using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Snippet
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "file_name")]
        [JsonPropertyName("file_name")]
        public string FileName;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "author")]
        [JsonPropertyName("author")]
        public Author Author;

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public string UpdatedAt;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;
    }
}
