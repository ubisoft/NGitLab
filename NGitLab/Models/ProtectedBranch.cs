using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProtectedBranch
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("push_access_levels")]
    public AccessLevelInfo[] PushAccessLevels { get; set; }

    [JsonPropertyName("merge_access_levels")]
    public AccessLevelInfo[] MergeAccessLevels { get; set; }

    [JsonPropertyName("allow_force_push")]
    public bool AllowForcePush { get; set; }

    [JsonPropertyName("code_owner_approval_required")]
    public bool CodeOwnerApprovalRequired { get; set; }
}
