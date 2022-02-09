using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Ref
    {
        [DataMember(Name = "type")]
        [JsonPropertyName("type")]
        public string Type;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;
    }
}
