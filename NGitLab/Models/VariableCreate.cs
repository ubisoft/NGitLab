using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class VariableCreate
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// The description of a variable
    /// </summary>
    /// <returns>The description of a variable</returns>
    /// <remarks>Introduced in GitLab 16.2</remarks>
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

    /// <summary>
    /// The environment scope of a variable
    /// </summary>
    /// <remarks>
    /// Create and Update of project variable: All tiers (Free, Premium, Ultimate).<br/>
    /// Create and Update of group variable: Premium and Ultimate only.
    /// </remarks>
    [JsonPropertyName("environment_scope")]
    public string EnvironmentScope { get; set; }
}
