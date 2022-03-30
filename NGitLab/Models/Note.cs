using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Note
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

        [JsonPropertyName("confidential")]
        public bool Confidential;

        [JsonPropertyName("noteable_iid")]
        public long NoteableIid { get; set; }

        [JsonPropertyName("noteable_type")]
        public NGitLab.DynamicEnum<NoteableType> NoteableType { get; set; }
    }
}
