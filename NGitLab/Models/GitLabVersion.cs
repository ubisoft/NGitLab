using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{Version,nq} r{Revision,nq}")]
public class GitLabVersion
{
    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("revision")]
    public string Revision { get; set; }
}
