using System.Diagnostics;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[DebuggerDisplay("{Path} ({Type})")]
public class Tree
{
    [JsonPropertyName("id")]
    public Sha1 Id;

    [JsonPropertyName("name")]
    public string Name;

    [JsonPropertyName("type")]
    public ObjectType Type;

    [JsonPropertyName("mode")]
    public string Mode;

    [JsonPropertyName("path")]
    public string Path;
}
