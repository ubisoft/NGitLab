using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class BranchProtect
{
    public BranchProtect(string branchName)
    {
        BranchName = branchName;
    }

    [JsonPropertyName("name")]
    public string BranchName { get; set; }

    [JsonPropertyName("push_access_level")]
    public AccessLevel? PushAccessLevel { get; set; } = null;

    [JsonPropertyName("merge_access_level")]
    public AccessLevel? MergeAccessLevel { get; set; } = null;

    [JsonPropertyName("unprotect_access_level")]
    public AccessLevel? UnprotectAccessLevel { get; set; } = null;

    [JsonPropertyName("allow_force_push")]
    public bool AllowForcePush { get; set; } = false;

    [JsonPropertyName("allowed_to_merge")]
    public AccessLevelInfo[] AllowedToMerge { get; set; }

    [JsonPropertyName("allowed_to_push")]
    public AccessLevelInfo[] AllowedToPush { get; set; }

    [JsonPropertyName("allowed_to_unprotect")]
    public AccessLevelInfo[] AllowedToUnprotect { get; set; }

    [JsonPropertyName("code_owner_approval_required")]
    public bool CodeOwnerApprovalRequired { get; set; } = false;
}
