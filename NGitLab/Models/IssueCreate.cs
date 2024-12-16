using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models;

public class IssueCreate
{
    [JsonIgnore]
    public long ProjectId { get; set; }

    [Required]
    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("assignee_id")]
    public long? AssigneeId;

    [JsonPropertyName("assignee_ids")]
    public long[] AssigneeIds;

    [JsonPropertyName("milestone_id")]
    public long? MileStoneId;

    [JsonPropertyName("labels")]
    public string Labels;

    [JsonPropertyName("confidential")]
    public bool Confidential;

    [JsonPropertyName("due_date")]
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? DueDate;

    [JsonPropertyName("epic_id")]
    public long? EpicId;
}
