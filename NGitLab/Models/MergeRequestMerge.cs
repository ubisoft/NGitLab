using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestMerge
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
    public bool? ShouldRemoveSourceBranch { get; set; }

    /// <summary>
    /// (optional) - if true the MR is merged when the pipeline succeeds
    /// </summary>
    [JsonPropertyName("merge_when_pipeline_succeeds")]
    public bool? MergeWhenPipelineSucceeds { get; set; }

    /// <summary>
    /// (optional) - if true the commits will be squashed into a single commit on merge
    /// </summary>
    [JsonPropertyName("squash")]
    public bool? Squash { get; set; }

    /// <summary>
    /// (optional) - if present, then this SHA must match the HEAD of the source branch, otherwise the merge will fail
    /// </summary>
    [JsonPropertyName("sha")]
    public string Sha { get; set; }
}
