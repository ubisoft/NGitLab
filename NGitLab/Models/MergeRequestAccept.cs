using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[Obsolete("You should use MergeRequestMerge instead of MergeRequestAccept")]
public class MergeRequestAccept
{
    /// <summary>
    /// (optional) - Custom merge commit message
    /// </summary>
    [JsonPropertyName("merge_commit_message")]
    public string MergeCommitMessage { get; set; }

    /// <summary>
    /// (optional) - if true removes the source branch
    /// </summary>
    [JsonPropertyName("should_remove_source_branch")]
    public bool ShouldRemoveSourceBranch { get; set; }

    /// <summary>
    /// (optional) - if true the MR is merged when the build succeeds
    /// </summary>
    [JsonPropertyName("merge_when_pipeline_succeeds")]
    public bool MergeWhenBuildSucceeds { get; set; }

    /// <summary>
    /// (optional) - if present, then this SHA must match the HEAD of the source branch, otherwise the merge will fail
    /// </summary>
    [JsonPropertyName("sha")]
    public string Sha { get; set; }
}
