using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectIssueNoteCreate
{
    [JsonIgnore]
    public long IssueId { get; set; }

    [Required]
    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("confidential")]
    public bool Confidential { get; set; }
}
