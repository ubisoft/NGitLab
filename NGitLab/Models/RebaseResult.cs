using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class RebaseResult
{
    [JsonPropertyName("rebase_in_progress")]
    public bool RebaseInProgress { get; set; }
}
