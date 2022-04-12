using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class MergeRequestApprovals
    {
        [JsonPropertyName("approvers")]
        public MergeRequestApprover[] Approvers;

        [JsonPropertyName("approvals_required")]
        public int ApprovalsRequired { get; set; }

        [JsonPropertyName("approvals_left")]
        public int ApprovalsLeft { get; set; }

        [JsonPropertyName("user_has_approved")]
        public bool UserHasApproved { get; set; }

        [JsonPropertyName("user_can_approve")]
        public bool UserCanApprove { get; set; }
    }

    public class MergeRequestApproversChange
    {
        private static readonly int[] EmptyIntArray = new int[0];

        [JsonPropertyName("approver_ids")]
        public int[] Approvers = EmptyIntArray;

        [JsonPropertyName("approver_group_ids")]
        public int[] ApproverGroups = EmptyIntArray;
    }

    public class MergeRequestApproveRequest
    {
        [JsonPropertyName("sha")]
        public string Sha { get; set; }

        [JsonPropertyName("approval_password")]
        public string ApprovalPassword { get; set; }
    }
}
