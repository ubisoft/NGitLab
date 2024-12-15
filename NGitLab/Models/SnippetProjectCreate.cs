using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SnippetProjectCreate
{
    public long ProjectId;

    [Required]
    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("description")]
    public string Description;

    [Required]
    [JsonPropertyName("visibility")]
    public VisibilityLevel Visibility;

    /// <summary>
    /// An array of snippet files. Required when updating snippets with multiple files.
    /// </summary>
    [JsonPropertyName("files")]
    public SnippetCreateFile[] Files { get; set; }
}
