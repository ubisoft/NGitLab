using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetsInfo
    {
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("sources")]
        public ReleaseAssetSource[] Sources { get; set; }

        [JsonPropertyName("links")]
        public ReleaseLink[] Links { get; set; }
    }
}
