using System.Runtime.Serialization;

namespace NGitLab.Models;

/// <summary>
/// Values that represent <see href="https://docs.gitlab.com/ee/api/merge_requests.html#merge-status">the 'detailed_merge_status' potential values</see>.
/// </summary>
public enum DetailedMergeStatus
{
    [EnumMember(Value = "blocked_status")]
    BlockedStatus,
    [EnumMember(Value = "broken_status")]
    BrokenStatus,
    [EnumMember(Value = "checking")]
    Checking,
    [EnumMember(Value = "unchecked")]
    Unchecked,
    [EnumMember(Value = "ci_must_pass")]
    CiMustPass,
    [EnumMember(Value = "ci_still_running")]
    CiStillRunning,
    [EnumMember(Value = "discussions_not_resolved")]
    DiscussionsNotResolved,
    [EnumMember(Value = "draft_status")]
    DraftStatus,
    [EnumMember(Value = "external_status_checks")]
    ExternalStatusChecks,
    [EnumMember(Value = "mergeable")]
    Mergeable,
    [EnumMember(Value = "not_approved")]
    NotApproved,
    [EnumMember(Value = "not_open")]
    NotOpen,
    [EnumMember(Value = "policies_denied")]
    PoliciesDenied,
    // Undocumented member
    [EnumMember(Value = "preparing")]
    Preparing,
}
