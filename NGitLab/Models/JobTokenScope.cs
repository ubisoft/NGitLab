using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class JobTokenScope
{
    /// <summary>
    /// Indicates if the "Limit access to this project" setting is enabled.
    /// If disabled, then all projects have access.
    /// </summary>
    [JsonPropertyName("inbound_enabled")]
    public bool InboundEnabled { get; set; }
}
