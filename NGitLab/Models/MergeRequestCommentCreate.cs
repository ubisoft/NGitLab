using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestCommentCreate
{
    [JsonPropertyName("body")]
    public string Body { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
}
