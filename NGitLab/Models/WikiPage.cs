using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class WikiPage
{
    [JsonPropertyName("content")]
    public string Content;

    [JsonPropertyName("format")]
    public string Format;

    [JsonPropertyName("slug")]
    public string Slug;

    [JsonPropertyName("title")]
    public string Title;
}
