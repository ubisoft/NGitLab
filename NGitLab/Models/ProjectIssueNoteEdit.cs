using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectIssueNoteEdit
{
    [JsonIgnore]
    public long IssueId { get; set; }

    [JsonIgnore]
    public long NoteId { get; set; }

    [Required]
    [JsonPropertyName("body")]
    public string Body { get; set; }
}
