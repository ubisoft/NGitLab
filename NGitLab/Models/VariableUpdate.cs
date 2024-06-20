using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class VariableUpdate
{
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// The description of a variable
    /// </summary>
    /// <returns>The description of a variable</returns>
    /// <remarks>Introduced in GitLab 16.2</remarks>
    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("protected")]
    public bool Protected;

    [JsonPropertyName("variable_type")]
    public VariableType Type;

    [JsonPropertyName("masked")]
    public bool Masked;

    [JsonPropertyName("raw")]
    public bool Raw;

    /// <summary>
    /// The environment scope of a variable
    /// </summary>
    /// <remarks>
    /// Create and Update of project variable: All tiers (Free, Premium, Ultimate).<br/>
    /// Create and Update of group variable: Premium and Ultimate only.
    /// </remarks>
    [JsonPropertyName("environment_scope")]
    public string EnvironmentScope;
}
