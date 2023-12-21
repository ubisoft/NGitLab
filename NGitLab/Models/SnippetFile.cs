using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SnippetFile
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("raw_url")]
    public string RawUrl { get; set; }
}
