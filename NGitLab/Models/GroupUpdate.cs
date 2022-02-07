using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class GroupUpdate
    {
        [Required]
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel? Visibility { get; set; }

        [DataMember(Name = "lfs_enabled")]
        [JsonPropertyName("lfs_enabled")]
        public bool? LfsEnabled { get; set; }

        [DataMember(Name = "request_access_enabled")]
        [JsonPropertyName("request_access_enabled")]
        public bool? RequestAccessEnabled { get; set; }

        [DataMember(Name = "shared_runners_minutes_limit")]
        [JsonPropertyName("shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit { get; set; }

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        [JsonPropertyName("extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit { get; set; }
    }
}
