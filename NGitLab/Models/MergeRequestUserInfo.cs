using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestUserInfo
{
    [JsonPropertyName("can_merge")]
    public bool CanMerge { get; set; }
}
