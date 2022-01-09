using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class DiffRefs
    {
        [DataMember(Name = "base_sha")]
        [JsonPropertyName("base_sha")]
        public string BaseSha { get; set; }

        [DataMember(Name = "head_sha")]
        [JsonPropertyName("head_sha")]
        public string HeadSha { get; set; }

        [DataMember(Name = "start_sha")]
        [JsonPropertyName("start_sha")]
        public string StartSha { get; set; }
    }
}
