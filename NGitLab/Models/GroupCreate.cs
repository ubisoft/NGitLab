using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class GroupCreate
    {
        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [Required]
        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description = string.Empty;

        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;

        [DataMember(Name = "lfs_enabled")]
        [JsonPropertyName("lfs_enabled")]
        public bool LfsEnabled;

        [DataMember(Name = "request_access_enabled")]
        [JsonPropertyName("request_access_enabled")]
        public bool RequestAccessEnabled;

        [DataMember(Name = "parent_id")]
        [JsonPropertyName("parent_id")]
        public int? ParentId;

        [DataMember(Name = "shared_runners_minutes_limit")]
        [JsonPropertyName("shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit { get; set; }

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        [JsonPropertyName("extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit { get; set; }
    }
}
