using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineVariable
    {
        [JsonPropertyName("key")]
        public string Key;

        [JsonPropertyName("value")]
        public string Value;

        [JsonPropertyName("variable_type")]
        public string VariableType;
    }
}
