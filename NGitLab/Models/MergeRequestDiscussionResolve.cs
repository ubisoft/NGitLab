using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestDiscussionResolve
{
    [JsonPropertyName("discussion_id")]
    public string Id;

    [JsonPropertyName("resolved")]
    public bool Resolved;
}
