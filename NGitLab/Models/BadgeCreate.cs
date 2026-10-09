using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class BadgeCreate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("link_url")]
    public string LinkUrl { get; set; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }
}
