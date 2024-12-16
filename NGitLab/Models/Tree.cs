using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{Path} ({Type})")]
public class Tree
{
    [JsonPropertyName("id")]
    public Sha1 Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public ObjectType Type { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }
}
