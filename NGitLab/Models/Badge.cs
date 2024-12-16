using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Badge
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("link_url")]
    public string LinkUrl { get; set; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("rendered_link_url")]
    public string RenderedLinkUrl { get; set; }

    [JsonPropertyName("rendered_image_url")]
    public string RenderedImageUrl { get; set; }

    [JsonPropertyName("kind")]
    public BadgeKind Kind { get; set; }
}
