using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineVariable
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("variable_type")]
    public string VariableType { get; set; }
}
