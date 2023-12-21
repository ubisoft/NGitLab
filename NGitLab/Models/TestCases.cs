using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class TestCases
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("classname")]
    public string Classname { get; set; }

    [JsonPropertyName("execution_time")]
    public int ExecutionTime { get; set; }

    [JsonPropertyName("system_ouput")]
    public string SystemOutput { get; set; }

    [JsonPropertyName("stack_trace")]
    public string StrackTrace { get; set; }
}
