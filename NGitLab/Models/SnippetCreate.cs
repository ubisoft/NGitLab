using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SnippetCreate
{
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("visibility")]
    public VisibilityLevel Visibility { get; set; }

    /// <summary>
    /// An array of snippet files. Required when updating snippets with multiple files.
    /// </summary>
    [JsonPropertyName("files")]
    public SnippetCreateFile[] Files { get; set; }
}
