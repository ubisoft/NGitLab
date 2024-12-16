using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestApprover
{
    [JsonPropertyName("user")]
    public User User { get; set; }
}
