using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectIssueNoteCreate
{
    [JsonIgnore]
    public long IssueId;

    [Required]
    [JsonPropertyName("body")]
    public string Body;

    [JsonPropertyName("confidential")]
    public bool Confidential;
}
