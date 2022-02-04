using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestComment
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public long Id;

        [DataMember(Name = "body")]
        [JsonPropertyName("body")]
        public string Body;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "author")]
        [JsonPropertyName("author")]
        public User Author { get; set; }

        [DataMember(Name = "resolved")]
        [JsonPropertyName("resolved")]
        public bool Resolved;

        [DataMember(Name = "resolvable")]
        [JsonPropertyName("resolvable")]
        public bool Resolvable;

        [DataMember(Name = "type")]
        [JsonPropertyName("type")]
        public string Type;

        [DataMember(Name = "system")]
        [JsonPropertyName("system")]
        public bool System;
    }
}
