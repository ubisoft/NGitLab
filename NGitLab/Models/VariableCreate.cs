using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class VariableCreate
    {
        [DataMember(Name = "key")]
        [JsonPropertyName("key")]
        public string Key;

        [DataMember(Name = "value")]
        [JsonPropertyName("value")]
        public string Value;

        [DataMember(Name = "protected")]
        [JsonPropertyName("protected")]
        public bool Protected;
    }
}
