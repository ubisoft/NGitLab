using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TestCases
    {
        [DataMember(Name = "status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember(Name = "classname")]
        [JsonPropertyName("classname")]
        public string Classname { get; set; }

        [DataMember(Name = "execution_time")]
        [JsonPropertyName("execution_time")]
        public int ExecutionTime { get; set; }

        [DataMember(Name = "system_ouput")]
        [JsonPropertyName("system_ouput")]
        public string SystemOutput { get; set; }

        [DataMember(Name = "stack_trace")]
        [JsonPropertyName("stack_trace")]
        public string StrackTrace { get; set; }
    }
}
