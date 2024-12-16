using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Note
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("author")]
    public User Author { get; set; }

    [JsonPropertyName("resolved")]
    public bool Resolved { get; set; }

    [JsonPropertyName("resolvable")]
    public bool Resolvable { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("system")]
    public bool System { get; set; }

    [JsonPropertyName("confidential")]
    public bool Confidential { get; set; }

    [JsonPropertyName("noteable_iid")]
    public long NoteableIid { get; set; }

    [JsonPropertyName("noteable_type")]
    public DynamicEnum<NoteableType> NoteableType { get; set; }
}
