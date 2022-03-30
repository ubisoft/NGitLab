using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Ref
    {
        [JsonPropertyName("type")]
        public string Type;

        [JsonPropertyName("name")]
        public string Name;
    }
}
