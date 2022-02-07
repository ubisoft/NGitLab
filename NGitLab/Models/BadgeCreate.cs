using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BadgeCreate
    {
        [DataMember(Name = "link_url")]
        [JsonPropertyName("link_url")]
        public string LinkUrl;

        [DataMember(Name = "image_url")]
        [JsonPropertyName("image_url")]
        public string ImageUrl;
    }
}
