using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestMerge
    {
        /// <summary>
        /// (optional) - Custom merge commit message
        /// </summary>
        [DataMember(Name = "merge_commit_message")]
        [JsonPropertyName("merge_commit_message")]
        public string MergeCommitMessage { get; set; }

        /// <summary>
        /// (optional) - if true removes the source branch
        /// </summary>
        [DataMember(Name = "should_remove_source_branch")]
        [JsonPropertyName("should_remove_source_branch")]
        public bool? ShouldRemoveSourceBranch { get; set; }

        /// <summary>
        /// (optional) - if true the MR is merged when the pipeline succeeds
        /// </summary>
        [DataMember(Name = "merge_when_pipeline_succeeds")]
        [JsonPropertyName("merge_when_pipeline_succeeds")]
        public bool? MergeWhenPipelineSucceeds { get; set; }

        /// <summary>
        /// (optional) - if true the commits will be squashed into a single commit on merge
        /// </summary>
        [DataMember(Name = "squash")]
        [JsonPropertyName("squash")]
        public bool? Squash { get; set; }

        /// <summary>
        /// (optional) - if present, then this SHA must match the HEAD of the source branch, otherwise the merge will fail
        /// </summary>
        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string Sha { get; set; }
    }
}
