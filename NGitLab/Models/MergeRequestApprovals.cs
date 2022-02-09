using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprovals
    {
        [DataMember(Name = "approvers")]
        [JsonPropertyName("approvers")]
        public MergeRequestApprover[] Approvers;

        [DataMember(Name = "approvals_required")]
        [JsonPropertyName("approvals_required")]
        public int ApprovalsRequired { get; set; }

        [DataMember(Name = "approvals_left")]
        [JsonPropertyName("approvals_left")]
        public int ApprovalsLeft { get; set; }

        [DataMember(Name = "user_has_approved")]
        [JsonPropertyName("user_has_approved")]
        public bool UserHasApproved { get; set; }

        [DataMember(Name = "user_can_approve")]
        [JsonPropertyName("user_can_approve")]
        public bool UserCanApprove { get; set; }
    }

    [DataContract]
    public class MergeRequestApproversChange
    {
        private static readonly int[] EmptyIntArray = new int[0];

        [DataMember(Name = "approver_ids")]
        [JsonPropertyName("approver_ids")]
        public int[] Approvers = EmptyIntArray;

        [DataMember(Name = "approver_group_ids")]
        [JsonPropertyName("approver_group_ids")]
        public int[] ApproverGroups = EmptyIntArray;
    }

    [DataContract]
    public class MergeRequestApproveRequest
    {
        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string Sha { get; set; }

        [DataMember(Name = "approval_password")]
        [JsonPropertyName("approval_password")]
        public string ApprovalPassword { get; set; }
    }
}
