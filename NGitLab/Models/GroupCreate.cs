using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class GroupCreate
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [Required]
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("visibility")]
    public VisibilityLevel Visibility { get; set; }

    [JsonPropertyName("lfs_enabled")]
    public bool LfsEnabled { get; set; }

    [JsonPropertyName("request_access_enabled")]
    public bool RequestAccessEnabled { get; set; }

    [JsonPropertyName("parent_id")]
    public long? ParentId { get; set; }

    [JsonPropertyName("shared_runners_minutes_limit")]
    public int? SharedRunnersMinutesLimit { get; set; }

    [JsonPropertyName("extra_shared_runners_minutes_limit")]
    public int? ExtraSharedRunnersMinutesLimit { get; set; }
}
