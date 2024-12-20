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
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("assignee_id")]
    public long? AssigneeId { get; set; }

    [JsonPropertyName("assignee_ids")]
    public long[] AssigneeIds { get; set; }

    [JsonPropertyName("milestone_id")]
    public long? MileStoneId { get; set; }

    [JsonPropertyName("labels")]
    public string Labels { get; set; }

    [JsonPropertyName("confidential")]
    public bool Confidential { get; set; }

    [JsonPropertyName("due_date")]
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("epic_id")]
    public long? EpicId { get; set; }

    [JsonPropertyName("weight")]
    public int? Weight { get; set; }
}
