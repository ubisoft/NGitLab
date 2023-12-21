using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class EpicCreate
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("labels")]
    public string Labels { get; set; }
}
