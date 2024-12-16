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
