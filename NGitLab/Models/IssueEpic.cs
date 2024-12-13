using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class IssueEpic
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("iid")]
    public long EpicId { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}
