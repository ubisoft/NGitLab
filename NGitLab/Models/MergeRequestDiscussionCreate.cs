using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestDiscussionCreate
{
    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; }
}
