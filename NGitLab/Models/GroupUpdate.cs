using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class GroupUpdate
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "visibility")]
        public VisibilityLevel? Visibility { get; set; }

        [DataMember(Name = "lfs_enabled")]
        public bool? LfsEnabled { get; set; }

        [DataMember(Name = "request_access_enabled")]
        public bool? RequestAccessEnabled { get; set; }

        [DataMember(Name = "shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit { get; set; }

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit { get; set; }
    }
}
