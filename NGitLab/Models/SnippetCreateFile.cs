using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SnippetCreateFile
{
    /// <summary>
    /// File path of the snippet file
    /// </summary>
    [Required]
    [JsonPropertyName("file_path")]
    public string FilePath { get; set; }

    /// <summary>
    /// Content of the snippet file
    /// </summary>
    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
