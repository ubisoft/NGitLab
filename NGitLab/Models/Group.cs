using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Group
    {
        public const string Url = "/groups";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "visibility")]
        [JsonPropertyName("visibility")]
        public VisibilityLevel Visibility;

        [DataMember(Name = "lfs_enabled")]
        [JsonPropertyName("lfs_enabled")]
        public bool LfsEnabled;

        [DataMember(Name = "avatar_url")]
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl;

        [DataMember(Name = "request_access_enabled")]
        [JsonPropertyName("request_access_enabled")]
        public bool RequestAccessEnabled;

        [DataMember(Name = "full_name")]
        [JsonPropertyName("full_name")]
        public string FullName;

        [DataMember(Name = "full_path")]
        [JsonPropertyName("full_path")]
        public string FullPath;

        [DataMember(Name = "parent_id")]
        [JsonPropertyName("parent_id")]
        public int? ParentId;

        [DataMember(Name = "projects")]
        [JsonPropertyName("projects")]
        public Project[] Projects;

        [DataMember(Name = "shared_runners_minutes_limit")]
        [JsonPropertyName("shared_runners_minutes_limit")]
        public int? SharedRunnersMinutesLimit;

        [DataMember(Name = "extra_shared_runners_minutes_limit")]
        [JsonPropertyName("extra_shared_runners_minutes_limit")]
        public int? ExtraSharedRunnersMinutesLimit;

        [DataMember(Name = "marked_for_deletion_on")]
        [JsonPropertyName("marked_for_deletion_on")]
        public string MarkedForDeletionOn;
    }
}
