using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProjectLabelDelete
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
