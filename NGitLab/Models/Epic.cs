using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Epic
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("iid")]
    public long EpicIid { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("labels")]
    public string[] Labels { get; set; }

    [JsonPropertyName("state")]
    public EpicState State { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }
}
