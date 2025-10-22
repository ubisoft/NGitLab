﻿using System;
using System.Runtime.Serialization;

namespace NGitLab.Models;

/// <summary>
/// Some of the possible <see href="https://docs.gitlab.com/api/merge_requests/#merge-status">detailed_merge_status</see> values.
/// </summary>
public enum DetailedMergeStatus
{
    [Obsolete("Not part of the GitLab API documentation: https://docs.gitlab.com/api/merge_requests/#merge-status")]
    [EnumMember(Value = "blocked_status")]
    BlockedStatus,
    [Obsolete("Not part of the GitLab API documentation: https://docs.gitlab.com/api/merge_requests/#merge-status")]
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
    [Obsolete("Not part of the GitLab API documentation: https://docs.gitlab.com/api/merge_requests/#merge-status")]
    [EnumMember(Value = "external_status_checks")]
    ExternalStatusChecks,
    [EnumMember(Value = "mergeable")]
    Mergeable,
    [EnumMember(Value = "not_approved")]
    NotApproved,
    [EnumMember(Value = "not_open")]
    NotOpen,
    [Obsolete("Not part of the GitLab API documentation: https://docs.gitlab.com/api/merge_requests/#merge-status")]
    [EnumMember(Value = "policies_denied")]
    PoliciesDenied,
    [EnumMember(Value = "preparing")]
    Preparing,
}
