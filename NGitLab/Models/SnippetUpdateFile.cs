using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public enum SnippetUpdateFileAction
{
    [EnumMember(Value = "create")]
    Create,
    [EnumMember(Value = "update")]
    Update,
    [EnumMember(Value = "delete")]
    Delete,
    [EnumMember(Value = "move")]
    Move,
}

public class SnippetUpdateFile
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

    /// <summary>
    /// Type of action to perform on the file, one of: create, update, delete, move
    /// </summary>
    [Required]
    [JsonPropertyName("action")]
    public SnippetUpdateFileAction Action { get; set; }

    /// <summary>
    /// Previous path of the snippet file
    /// </summary>
    [JsonPropertyName("previous_path")]
    public string PreviousFile { get; set; }
}
