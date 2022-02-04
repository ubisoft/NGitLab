using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseLinkUpdate
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [DataMember(Name = "filepath")]
        [JsonPropertyName("filepath")]
        public string Filepath { get; set; }

        [DataMember(Name = "link_type")]
        [JsonPropertyName("link_type")]
        public ReleaseLinkType LinkType { get; set; }
    }
}
