﻿using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestApprovals
{
    [JsonPropertyName("approvers")]
    public MergeRequestApprover[] Approvers;

    [JsonPropertyName("approved_by")]
    public MergeRequestApprover[] ApprovedBy { get; set; }

    [JsonPropertyName("approved")]
    public bool Approved { get; set; }

    [JsonPropertyName("approvals_required")]
    public int ApprovalsRequired { get; set; }

    [JsonPropertyName("approvals_left")]
    public int ApprovalsLeft { get; set; }

    [JsonPropertyName("user_has_approved")]
    public bool UserHasApproved { get; set; }

    [JsonPropertyName("user_can_approve")]
    public bool UserCanApprove { get; set; }

    [JsonPropertyName("suggested_approvers")]
    public User[] SuggestedApprovers { get; set; }
}

public class MergeRequestApproversChange
{
    [JsonPropertyName("approver_ids")]
    public int[] Approvers = Array.Empty<int>();

    [JsonPropertyName("approver_group_ids")]
    public int[] ApproverGroups = Array.Empty<int>();
}

public class MergeRequestApproveRequest
{
    [JsonPropertyName("sha")]
    public string Sha { get; set; }

    [JsonPropertyName("approval_password")]
    public string ApprovalPassword { get; set; }
}
