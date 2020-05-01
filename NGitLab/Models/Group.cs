using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Group
    {
        public const string Url = "/groups";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "path")]
        public string Path;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "visibility")]
        public VisibilityLevel Visibility;

        [DataMember(Name = "lfs_enabled")]
        public bool LfsEnabled;

        [DataMember(Name = "avatar_url")]
        public string AvatarUrl;

        [DataMember(Name = "request_access_enabled")]
        public bool RequestAccessEnabled;

        [DataMember(Name = "full_name")]
        public string FullName;

        [DataMember(Name = "full_path")]
        public string FullPath;

        [DataMember(Name = "parent_id")]
        public int? ParentId;

        [DataMember(Name = "projects")]
        public Project[] Projects;

        [DataMember(Name = "shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit;

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit;

        [DataMember(Name = "marked_for_deletion_on")]
        public string MarkedForDeletionOn;
    }
}
