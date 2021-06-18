using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProtectedBranch
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "push_access_levels")]
        public AccessLevelInfo[] PushAccessLevels { get; set; }

        [DataMember(Name = "merge_access_levels")]
        public AccessLevelInfo[] MergeAccessLevels { get; set; }

        [DataMember(Name = "allow_force_push")]
        public bool AllowForcePush { get; set; }

        [DataMember(Name = "code_owner_approval_required")]
        public bool CodeOwnerApprovalRequired { get; set; }
    }
}
