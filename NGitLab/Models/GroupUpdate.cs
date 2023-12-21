using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class GroupUpdate
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [Required]
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("visibility")]
    public VisibilityLevel? Visibility { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool? LfsEnabled { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool? RequestAccessEnabled { get; set; }

    [JsonPropertyName("shared_runners_minutes_limit")]
    public int? SharedRunnersMinutesLimit { get; set; }

    [JsonPropertyName("extra_shared_runners_minutes_limit")]
    public int? ExtraSharedRunnersMinutesLimit { get; set; }
}
