using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ApprovalRuleUpdate
{
    /// <summary>
    /// The name of the approval rule.
    /// </summary>
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// ID of the approval rule.
    /// </summary>
    [Required]
    [JsonPropertyName("approval_rule_id")]
    public long ApprovalRuleId { get; set; }

    /// <summary>
    /// The number of approvals required.
    /// </summary>
    [Required]
    [JsonPropertyName("approvals_required")]
    public int ApprovalsRequired { get; set; }

    /// <summary>
    /// The ids of users as approvers.
    /// </summary>
    [JsonPropertyName("user_ids")]
    public long[] UserIds { get; set; }

    /// <summary>
    /// The ids of groups as approvers.
    /// </summary>
    [JsonPropertyName("group_ids")]
    public long[] GroupIds { get; set; }

    /// <summary>
    /// The ids of protected branches to scope the rule by.
    /// </summary>
    [JsonPropertyName("protected_branch_ids")]
    public long[] ProtectedBranchIds { get; set; }
}
