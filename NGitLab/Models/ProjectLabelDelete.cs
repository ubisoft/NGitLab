using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProjectLabelDelete
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
