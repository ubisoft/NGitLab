using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class WikiPage
{
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}
