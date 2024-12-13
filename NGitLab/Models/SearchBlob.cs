using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class SearchBlob
{
    [JsonPropertyName("basename")]
    public string BaseName { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("ref")]
    public Sha1 Ref { get; set; }

    [JsonPropertyName("startline")]
    public int StartLine { get; set; }

    [JsonPropertyName("project_id")]
    public long ProjectId { get; set; }
}
