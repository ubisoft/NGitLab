using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Epic
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        [JsonPropertyName("iid")]
        public int EpicIid { get; set; }

        [DataMember(Name = "group_id")]
        [JsonPropertyName("group_id")]
        public int GroupId { get; set; }

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string[] Labels { get; set; }

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public EpicState State { get; set; }

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl { get; set; }
    }
}
