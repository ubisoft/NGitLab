﻿using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprovals
    {
        [DataMember(Name = "approvers")]
        public MergeRequestApprover[] Approvers;

        [DataMember(Name = "approvals_required")]
        public int ApprovalsRequired { get; set; }

        [DataMember(Name = "approvals_left")]
        public int ApprovalsLeft { get; set; }

        [DataMember(Name = "user_has_approved")]
        public bool UserHasApproved { get; set; }

        [DataMember(Name = "user_can_approve")]
        public bool UserCanApprove { get; set; }
    }

    [DataContract]
    public class MergeRequestApproversChange
    {
        private static readonly int[] EmptyIntArray = new int[0];

        [DataMember(Name = "approver_ids")]
        public int[] Approvers = EmptyIntArray;

        [DataMember(Name = "approver_group_ids")]
        public int[] ApproverGroups = EmptyIntArray;
    }

    [DataContract]
    public class MergeRequestApproveRequest
    {
        [DataMember(Name = "sha")]
        public string Sha { get; set; }

        [DataMember(Name = "approval_password")]
        public string ApprovalPassword { get; set; }
    }
}
