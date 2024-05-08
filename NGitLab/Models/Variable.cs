using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Variable
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("protected")]
    public bool Protected { get; set; }

    [JsonPropertyName("variable_type")]
    public VariableType Type { get; set; }

    [JsonPropertyName("masked")]
    public bool Masked { get; set; }

    [JsonPropertyName("raw")]
    public bool Raw { get; set; }

    [JsonPropertyName("environment_scope")]
    public string Scope { get; set; }
}
