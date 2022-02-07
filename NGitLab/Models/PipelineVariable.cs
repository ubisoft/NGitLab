using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineVariable
    {
        [DataMember(Name = "key")]
        [JsonPropertyName("key")]
        public string Key;

        [DataMember(Name = "value")]
        [JsonPropertyName("value")]
        public string Value;

        [DataMember(Name = "variable_type")]
        [JsonPropertyName("variable_type")]
        public string VariableType;
    }
}
