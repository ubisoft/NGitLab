using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectIssueNoteEdit
{
    [JsonIgnore]
    public long IssueId;

    [JsonIgnore]
    public long NoteId;

    [Required]
    [JsonPropertyName("body")]
    public string Body;
}
