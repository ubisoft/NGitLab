using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class IssueClone
{
    [JsonPropertyName("to_project_id")]
    public long ToProjectId { get; set; }

    [JsonPropertyName("with_notes")]
    public bool WithNotes { get; set; }
}
