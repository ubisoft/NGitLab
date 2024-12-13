using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SnippetUpdate
{
    [Required]
    [JsonPropertyName("id")]
    public long SnippetId { get; set; }

    [Required]
    [JsonPropertyName("title")]
    public string Title;

    [Required]
    [JsonPropertyName("file_name")]
    [Obsolete("Consider using the Files array that support more than one file.")]
    public string FileName;

    [Required]
    [JsonPropertyName("content")]
    [Obsolete("Consider using the Files array that support more than one file.")]
    public string Content;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("visibility")]
    public VisibilityLevel Visibility;

    /// <summary>
    /// An array of snippet files. Required when updating snippets with multiple files.
    /// </summary>
    [JsonPropertyName("files")]
    public SnippetUpdateFile[] Files { get; set; }
}
