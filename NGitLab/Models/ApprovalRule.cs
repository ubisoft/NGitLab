using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ApprovalRule
{
    /// <summary>
    /// The rule Id.
    /// </summary>
    [JsonPropertyName("id")]
    public long RuleId { get; set; }

    /// <summary>
    /// The name of the approval rule.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The type of the approval rule.
    /// </summary>
    [JsonPropertyName("rule_type")]
    public string RuleType { get; set; }

    /// <summary>
    /// The eligible approvers for the rule.
    /// </summary>
    [JsonPropertyName("eligible_approvers")]
    public User[] EligibleApprovers { get; set; }

    /// <summary>
    /// The number of required approvers.
    /// </summary>
    [JsonPropertyName("approvals_required")]
    public int ApprovalsRequired { get; set; }

    /// <summary>
    /// The users as approvers.
    /// </summary>
    [JsonPropertyName("users")]
    public User[] Users { get; set; }

    /// <summary>
    /// The groups as approvers.
    /// </summary>
    [JsonPropertyName("groups")]
    public Group[] Groups { get; set; }

    /// <summary>
    /// The protected branches to scope the rule by.
    /// </summary>
    [JsonPropertyName("protected_branches")]
    public Branch[] ProtectedBranch { get; set; }
}
