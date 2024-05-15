using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[Obsolete($"Use {nameof(Variable)} instead!")]
public class VariableCreate
{
    [JsonPropertyName("key")]
    public string Key;

    [JsonPropertyName("value")]
    public string Value;

    [JsonPropertyName("protected")]
    public bool Protected;
}
