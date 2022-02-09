using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BranchProtect
    {
        public BranchProtect(string branchName)
        {
            BranchName = branchName;
        }

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string BranchName { get; set; }

        [DataMember(Name = "push_access_level")]
        [JsonPropertyName("push_access_level")]
        public AccessLevel? PushAccessLevel { get; set; } = null;

        [DataMember(Name = "merge_access_level")]
        [JsonPropertyName("merge_access_level")]
        public AccessLevel? MergeAccessLevel { get; set; } = null;

        [DataMember(Name = "unprotect_access_level")]
        [JsonPropertyName("unprotect_access_level")]
        public AccessLevel? UnprotectAccessLevel { get; set; } = null;

        [DataMember(Name = "allow_force_push")]
        [JsonPropertyName("allow_force_push")]
        public bool AllowForcePush { get; set; } = false;

        [DataMember(Name = "allowed_to_merge")]
        [JsonPropertyName("allowed_to_merge")]
        public AccessLevelInfo[] AllowedToMerge { get; set; }

        [DataMember(Name = "allowed_to_push")]
        [JsonPropertyName("allowed_to_push")]
        public AccessLevelInfo[] AllowedToPush { get; set; }

        [DataMember(Name = "allowed_to_unprotect")]
        [JsonPropertyName("allowed_to_unprotect")]
        public AccessLevelInfo[] AllowedToUnprotect { get; set; }

        [DataMember(Name = "code_owner_approval_required")]
        [JsonPropertyName("code_owner_approval_required")]
        public bool CodeOwnerApprovalRequired { get; set; } = false;
    }
}
