using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestCreate
    {
        [DataMember(Name = "source_branch")]
        [JsonPropertyName("source_branch")]
        public string SourceBranch;

        [DataMember(Name = "target_branch")]
        [JsonPropertyName("target_branch")]
        public string TargetBranch;

        [DataMember(Name = "assignee_id")]
        [JsonPropertyName("assignee_id")]
        public int? AssigneeId;

        [DataMember(Name = "assignee_ids")]
        [JsonPropertyName("assignee_ids")]
        public int[] AssigneeIds;

        [DataMember(Name = "reviewer_ids")]
        [JsonPropertyName("reviewer_ids")]
        public int[] ReviewerIds;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "target_project_id")]
        [JsonPropertyName("target_project_id")]
        public int? TargetProjectId;

        [DataMember(Name = "remove_source_branch")]
        [JsonPropertyName("remove_source_branch")]
        public bool RemoveSourceBranch;

        [DataMember(Name = "milestone_id")]
        [JsonPropertyName("milestone_id")]
        public int? MilestoneId { get; set; }

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string Labels;

        [DataMember(Name = "squash")]
        [JsonPropertyName("squash")]
        public bool Squash;

        [DataMember(Name = "allow_collaboration")]
        [JsonPropertyName("allow_collaboration")]
        public bool? AllowCollaboration;
    }
}
