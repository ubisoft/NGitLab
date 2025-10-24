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

    [JsonPropertyName("file")]
    public string File { get; set; }

    [JsonPropertyName("execution_time")]
    public double ExecutionTime { get; set; }

    [JsonPropertyName("system_output")]
    public string SystemOutput { get; set; }

    [JsonPropertyName("stack_trace")]
    public string StrackTrace { get; set; }
}
