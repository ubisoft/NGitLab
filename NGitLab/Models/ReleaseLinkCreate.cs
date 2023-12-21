using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ReleaseLinkCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("filepath")]
    public string Filepath { get; set; }

    [JsonPropertyName("link_type")]
    public ReleaseLinkType LinkType { get; set; }
}
