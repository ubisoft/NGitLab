using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BranchProtect
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "push_access_level")]
        public AccessLevel? PushAccessLevel { get; set; } = null;

        [DataMember(Name = "merge_access_level")]
        public AccessLevel? MergeAccessLevel { get; set; } = null;

        [DataMember(Name = "unprotect_access_level")]
        public AccessLevel? UnprotectAccessLevel { get; set; } = null;

        [DataMember(Name = "allow_force_push")]
        public bool AllowForcePush { get; set; } = false;

        [DataMember(Name = "allowed_to_merge")]
        public AccessLevelInfo[] AllowedToMerge { get; set; }

        [DataMember(Name = "allowed_to_push")]
        public AccessLevelInfo[] AllowedToPush { get; set; }

        [DataMember(Name = "allowed_to_unprotect")]
        public AccessLevelInfo[] AllowedToUnprotect { get; set; }

        [DataMember(Name = "code_owner_approval_required")]
        public bool CodeOwnerApprovalRequired { get; set; } = false;
    }
}
