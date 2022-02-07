using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetsInfo
    {
        [DataMember(Name = "count")]
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [DataMember(Name = "sources")]
        [JsonPropertyName("sources")]
        public ReleaseAssetSource[] Sources { get; set; }

        [DataMember(Name = "links")]
        [JsonPropertyName("links")]
        public ReleaseLink[] Links { get; set; }
    }
}
