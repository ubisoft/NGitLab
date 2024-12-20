using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class CommitRevert
{
    [Required]
    [JsonIgnore]
    public Sha1 Sha { get; set; }

    [Required]
    [JsonPropertyName("branch")]
    public string Branch { get; set; }

    [JsonPropertyName("dry_run")]
    public bool? DryRun { get; set; }
}
