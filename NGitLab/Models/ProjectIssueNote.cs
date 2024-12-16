using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectIssueNote
{
    [JsonPropertyName("id")]
    public long NoteId { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("attachment")]
    public string Attachment { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("system")]
    public bool System { get; set; }

    [JsonPropertyName("noteable_id")]
    public long NoteableId { get; set; }

    [JsonPropertyName("noteable_type")]
    public string NoteableType { get; set; }

    [JsonPropertyName("noteable_iid")]
    public long Noteable_Iid { get; set; }

    [JsonPropertyName("resolvable")]
    public bool Resolvable { get; set; }

    [JsonPropertyName("confidential")]
    public bool Confidential { get; set; }
}
