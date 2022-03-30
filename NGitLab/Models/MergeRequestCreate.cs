using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
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
    }
}
