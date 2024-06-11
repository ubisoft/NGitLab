using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[EditorBrowsable(EditorBrowsableState.Never)]
public class VariableUpdate
{
    [JsonPropertyName("value")]
    public string Value;

    [JsonPropertyName("protected")]
    public bool Protected;
}
