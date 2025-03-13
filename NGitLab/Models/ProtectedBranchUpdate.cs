using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class ProtectedBranchUpdate
{
    [JsonPropertyName("allowed_to_push")]
    public AccessLevelUpdate[] AllowedToPush { get; set; }

    [JsonPropertyName("allowed_to_merge")]
    public AccessLevelUpdate[] AllowedToMerge { get; set; }

    [JsonPropertyName("allow_force_push")]
    public bool? AllowForcePush { get; set; }

    [JsonPropertyName("code_owner_approval_required")]
    public bool? CodeOwnerApprovalRequired { get; set; }
}
