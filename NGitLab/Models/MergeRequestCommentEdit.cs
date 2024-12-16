using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestCommentEdit
{
    [JsonPropertyName("body")]
    public string Body { get; set; }
}
