using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseAssetSource
    {
        [DataMember(Name = "format")]
        [JsonPropertyName("format")]
        public string Format { get; set; }

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
