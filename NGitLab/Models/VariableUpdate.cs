using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class VariableUpdate
    {
        [JsonPropertyName("value")]
        public string Value;

        [JsonPropertyName("protected")]
        public bool Protected;
    }
}
