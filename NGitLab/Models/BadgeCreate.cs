using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class BadgeCreate
    {
        [JsonPropertyName("link_url")]
        public string LinkUrl;

        [JsonPropertyName("image_url")]
        public string ImageUrl;
    }
}
