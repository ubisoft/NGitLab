using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Label
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "color")]
        [JsonPropertyName("color")]
        public string Color;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;
    }
}
