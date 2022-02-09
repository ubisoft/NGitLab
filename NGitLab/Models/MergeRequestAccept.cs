using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    [Obsolete("You should use MergeRequestMerge instead of MergeRequestAccept")]
    public class MergeRequestAccept
    {
        /// <summary>
        /// (optional) - Custom merge commit message
        /// </summary>
        [DataMember(Name = "merge_commit_message")]
        [JsonPropertyName("merge_commit_message")]
        public string MergeCommitMessage;

        /// <summary>
        /// (optional) - if true removes the source branch
        /// </summary>
        [DataMember(Name = "should_remove_source_branch")]
        [JsonPropertyName("should_remove_source_branch")]
        public bool ShouldRemoveSourceBranch;

        /// <summary>
        /// (optional) - if true the MR is merged when the build succeeds
        /// </summary>
        [DataMember(Name = "merge_when_pipeline_succeeds")]
        [JsonPropertyName("merge_when_pipeline_succeeds")]
        public bool MergeWhenBuildSucceeds;

        /// <summary>
        /// (optional) - if present, then this SHA must match the HEAD of the source branch, otherwise the merge will fail
        /// </summary>
        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string Sha;
    }
}
