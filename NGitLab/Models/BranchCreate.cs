using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class BranchCreate
{
    [JsonPropertyName("branch")]
    public string Name { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }
}
