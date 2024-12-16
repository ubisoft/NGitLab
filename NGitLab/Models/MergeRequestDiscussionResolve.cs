using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestDiscussionResolve
{
    [JsonPropertyName("discussion_id")]
    public string Id { get; set; }

    [JsonPropertyName("resolved")]
    public bool Resolved { get; set; }
}
