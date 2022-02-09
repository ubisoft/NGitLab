using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestUpdate
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

        [DataMember(Name = "state_event")]
        [JsonPropertyName("state_event")]
        public string NewState;

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string Labels;

        [DataMember(Name = "milestone_id")]
        [JsonPropertyName("milestone_id")]
        public int? MilestoneId;

        [DataMember(Name = "allow_collaboration")]
        [JsonPropertyName("allow_collaboration")]
        public bool? AllowCollaboration;
    }

    [DataContract]
    public class MergeRequestUpdateState
    {
        [DataMember(Name = "state_event")]
        [JsonPropertyName("state_event")]
        public string NewState;
    }

    // ReSharper disable InconsistentNaming
    public enum MergeRequestStateEvent
    {
        close,
        reopen,
        merge,
    }
}
