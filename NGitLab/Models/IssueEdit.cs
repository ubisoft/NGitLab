using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models
{
    public class IssueEdit
    {
        [JsonIgnore]
        public int ProjectId { get => Id; set => Id = value; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [JsonIgnore]
        public int Id;

        [Required]
        [JsonPropertyName("issue_id")]
        public int IssueId;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("assignee_id")]
        public int? AssigneeId;

        [JsonPropertyName("assignee_ids")]
        public int[] AssigneeIds;

        [JsonPropertyName("milestone_id")]
        public int? MilestoneId;

        [JsonPropertyName("labels")]
        public string Labels;

        [JsonPropertyName("state_event")]
        public string State;

        [JsonPropertyName("due_date")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? DueDate;

        [JsonPropertyName("epic_id")]
        public int? EpicId;
    }
}
