using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Badge
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "link_url")]
        [JsonPropertyName("link_url")]
        public string LinkUrl;

        [DataMember(Name = "image_url")]
        [JsonPropertyName("image_url")]
        public string ImageUrl;

        [DataMember(Name = "rendered_link_url")]
        [JsonPropertyName("rendered_link_url")]
        public string RenderedLinkUrl;

        [DataMember(Name = "rendered_image_url")]
        [JsonPropertyName("rendered_image_url")]
        public string RenderedImageUrl;

        [DataMember(Name = "kind")]
        [JsonPropertyName("kind")]
        public BadgeKind Kind;
    }
}
