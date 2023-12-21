using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Snippet
{
    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("file_name")]
    [Obsolete("Consider using the Files array that support more than one file.")]
    public string FileName;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("author")]
    public Author Author;

    [JsonPropertyName("updated_at")]
    public string UpdatedAt;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("web_url")]
    public string WebUrl;

    [JsonPropertyName("files")]
    public SnippetFile[] Files { get; set; }
}
