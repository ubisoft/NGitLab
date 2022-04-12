using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class GroupCreate
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [JsonPropertyName("path")]
        public string Path;

        [JsonPropertyName("description")]
        public string Description = string.Empty;

        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;

        [JsonPropertyName("lfs_enabled")]
        public bool LfsEnabled;

        [JsonPropertyName("request_access_enabled")]
        public bool RequestAccessEnabled;

        [JsonPropertyName("parent_id")]
        public int? ParentId;

        [JsonPropertyName("shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit { get; set; }

        [JsonPropertyName("extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit { get; set; }
    }
}
