using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models
{
    public class IssueEdit
    {
        [Obsolete("Use " + nameof(ProjectId) + " instead")]
        [JsonIgnore]
        public int Id;

#pragma warning disable CS0618 // Type or member is obsolete
        [JsonIgnore]
        public int ProjectId { get => Id; set => Id = value; }
#pragma warning restore CS0618 // Type or member is obsolete

        [Required]
        [JsonPropertyName("issue_id")]
        public int IssueId;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("assignee_id")]
        public int? AssigneeId;

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
