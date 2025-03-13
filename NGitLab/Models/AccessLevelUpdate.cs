using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class AccessLevelUpdate
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("_destroy")]
    public bool? Destroy { get; set; }

    [JsonPropertyName("access_level")]
    public AccessLevel? AccessLevel { get; set; }

    [JsonPropertyName("access_level_description")]
    public string Description { get; set; }

    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }

    [JsonPropertyName("group_id")]
    public int? GroupId { get; set; }
}