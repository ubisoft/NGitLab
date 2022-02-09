using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class VariableUpdate
    {
        [DataMember(Name = "value")]
        [JsonPropertyName("value")]
        public string Value;

        [DataMember(Name = "protected")]
        [JsonPropertyName("protected")]
        public bool Protected;
    }
}
