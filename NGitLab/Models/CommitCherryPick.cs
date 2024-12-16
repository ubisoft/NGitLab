using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CommitCherryPick
{
    [Required]
    [JsonIgnore]
    public Sha1 Sha { get; set; }

    [Required]
    [JsonPropertyName("branch")]
    public string Branch { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
