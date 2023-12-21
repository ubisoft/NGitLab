using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Badge
{
    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("link_url")]
    public string LinkUrl;

    [JsonPropertyName("image_url")]
    public string ImageUrl;

    [JsonPropertyName("rendered_link_url")]
    public string RenderedLinkUrl;

    [JsonPropertyName("rendered_image_url")]
    public string RenderedImageUrl;

    [JsonPropertyName("kind")]
    public BadgeKind Kind;
}
