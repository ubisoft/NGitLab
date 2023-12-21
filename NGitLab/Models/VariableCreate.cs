using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class VariableCreate
{
    [JsonPropertyName("key")]
    public string Key;

    [JsonPropertyName("value")]
    public string Value;

    [JsonPropertyName("protected")]
    public bool Protected;
}
