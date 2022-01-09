using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class EpicCreate
    {
        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string Labels { get; set; }
    }
}
