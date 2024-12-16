using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class FileDelete
{
    [Required]
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [Required]
    [JsonPropertyName("branch")]
    public string Branch { get; set; }

    [Required]
    [JsonPropertyName("commit_message")]
    public string CommitMessage { get; set; }
}
