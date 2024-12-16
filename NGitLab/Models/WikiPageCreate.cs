using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class WikiPageCreate
{
    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}
