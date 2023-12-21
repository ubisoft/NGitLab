using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class UploadedProjectFile
{
    [JsonPropertyName("alt")]
    public string Alt { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("full_path")]
    public string FullPath { get; set; }

    [JsonPropertyName("markdown")]
    public string Markdown { get; set; }
}
