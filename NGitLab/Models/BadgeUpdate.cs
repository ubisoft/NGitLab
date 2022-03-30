using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BadgeUpdate
    {
        [JsonPropertyName("link_url")]
        public string LinkUrl;

        [JsonPropertyName("image_url")]
        public string ImageUrl;
    }
}
