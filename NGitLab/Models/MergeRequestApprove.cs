using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestApprove
{
    /// <summary>
    /// (optional) - if present, then this SHA must match the HEAD of the source branch, otherwise the merge will fail
    /// </summary>
    [JsonPropertyName("sha")]
    public string Sha { get; set; }

    /// <summary>
    /// (optional) - Current user’s password. Required if Require user password to approve is enabled in the project settings.
    /// </summary>
    [JsonPropertyName("approval_password")]
    public string ApprovalPassword { get; set; }
}
