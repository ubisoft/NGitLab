using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[Obsolete($"Use {nameof(Variable)} instead!")]
public class VariableUpdate
{
    [JsonPropertyName("value")]
    public string Value;

    [JsonPropertyName("protected")]
    public bool Protected;
}
