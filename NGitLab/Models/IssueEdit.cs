using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models;

public class IssueEdit
{
    [JsonIgnore]
    public long ProjectId { get; set; }

    [Required]
    [JsonPropertyName("issue_id")]
    public long IssueId;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("assignee_id")]
    public long? AssigneeId;

    [JsonPropertyName("assignee_ids")]
    public long[] AssigneeIds;

    [JsonPropertyName("milestone_id")]
    public long? MilestoneId;

    [JsonPropertyName("labels")]
    public string Labels;

    [JsonPropertyName("state_event")]
    public string State;

    [JsonPropertyName("due_date")]
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? DueDate;

    [JsonPropertyName("epic_id")]
    public long? EpicId;
}
