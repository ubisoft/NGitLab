using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Variable
    {
        [DataMember(Name = "key")]
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [DataMember(Name = "protected")]
        [JsonPropertyName("protected")]
        public bool Protected { get; set; }

        [DataMember(Name = "variable_type")]
        [JsonPropertyName("variable_type")]
        public VariableType Type { get; set; }

        [DataMember(Name = "masked")]
        [JsonPropertyName("masked")]
        public bool Masked { get; set; }

        [DataMember(Name = "environment_scope")]
        [JsonPropertyName("environment_scope")]
        public string Scope { get; set; }
    }
}
