using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class MergeRequestCreate
    {
        [JsonPropertyName("source_branch")]
        public string SourceBranch;

        [JsonPropertyName("target_branch")]
        public string TargetBranch;

        [JsonPropertyName("assignee_id")]
        public int? AssigneeId;

        [JsonPropertyName("assignee_ids")]
        public int[] AssigneeIds;

        [JsonPropertyName("reviewer_ids")]
        public int[] ReviewerIds;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("target_project_id")]
        public int? TargetProjectId;

        [JsonPropertyName("remove_source_branch")]
        public bool RemoveSourceBranch;

        [JsonPropertyName("milestone_id")]
        public int? MilestoneId { get; set; }

        [JsonPropertyName("labels")]
        public string Labels;

        [JsonPropertyName("squash")]
        public bool Squash;

        [JsonPropertyName("allow_collaboration")]
        public bool? AllowCollaboration;

        /// <summary>
        /// Indicates whether the MR creation method should wait for GitLab to fully assess MR data before returning to the caller.
        /// This could be necessary in certain cases, starting with GitLab versions somewhere between 15.4 and 15.11...
        /// </summary>
        /// <remarks>
        /// Needed by NGitLab tests, as MRs are often created and immediately fetched.
        /// </remarks>
        [JsonIgnore]
        public bool AwaitAssessment { get; set; }
    }
}
