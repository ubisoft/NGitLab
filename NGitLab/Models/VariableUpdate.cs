using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class VariableUpdate
{
    [JsonPropertyName("value")]
    public string Value;

    [JsonPropertyName("protected")]
    public bool Protected;
}
