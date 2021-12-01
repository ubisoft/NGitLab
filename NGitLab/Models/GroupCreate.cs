using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class GroupCreate
    {
        [Required]
        [DataMember(Name = "name")]
        public string Name;

        [Required]
        [DataMember(Name = "path")]
        public string Path;

        [DataMember(Name = "description")]
        public string Description = string.Empty;

        [DataMember(Name = "visibility")]
        public VisibilityLevel Visibility;

        [DataMember(Name = "lfs_enabled")]
        public bool LfsEnabled;

        [DataMember(Name = "request_access_enabled")]
        public bool RequestAccessEnabled;

        [DataMember(Name = "parent_id")]
        public int? ParentId;

        [DataMember(Name = "shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit { get; set; }

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit { get; set; }
    }
}
