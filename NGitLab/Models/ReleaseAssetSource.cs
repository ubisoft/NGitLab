using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetSource
    {
        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
