using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestChange
{
    [JsonPropertyName("changes")]
    public Change[] Changes { get; set; }
}
