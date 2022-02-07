using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectIssueNote
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int NoteId;

        [DataMember(Name = "body")]
        [JsonPropertyName("body")]
        public string Body;

        [DataMember(Name = "attachment")]
        [JsonPropertyName("attachment")]
        public string Attachment;

        [DataMember(Name = "author")]
        [JsonPropertyName("author")]
        public Author Author;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "system")]
        [JsonPropertyName("system")]
        public bool System;

        [DataMember(Name = "noteable_id")]
        [JsonPropertyName("noteable_id")]
        public int NoteableId;

        [DataMember(Name = "noteable_type")]
        [JsonPropertyName("noteable_type")]
        public string NoteableType;

        [DataMember(Name = "noteable_iid")]
        [JsonPropertyName("noteable_iid")]
        public int Noteable_Iid;

        [DataMember(Name = "resolvable")]
        [JsonPropertyName("resolvable")]
        public bool Resolvable;

        [DataMember(Name = "confidential")]
        [JsonPropertyName("confidential")]
        public bool Confidential;
    }
}
