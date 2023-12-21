using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectIssueNoteEdit
{
    [JsonIgnore]
    public int IssueId;

    [JsonIgnore]
    public int NoteId;

    [Required]
    [JsonPropertyName("body")]
    public string Body;
}
