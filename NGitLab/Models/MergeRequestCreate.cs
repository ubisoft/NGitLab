using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestCreate
{
    [JsonPropertyName("source_branch")]
    public string SourceBranch { get; set; }

    [JsonPropertyName("target_branch")]
    public string TargetBranch { get; set; }

    [JsonPropertyName("assignee_id")]
    public long? AssigneeId { get; set; }

    [JsonPropertyName("assignee_ids")]
    public long[] AssigneeIds { get; set; }

    [JsonPropertyName("reviewer_ids")]
    public long[] ReviewerIds { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("target_project_id")]
    public long? TargetProjectId { get; set; }

    [JsonPropertyName("remove_source_branch")]
    public bool RemoveSourceBranch { get; set; }

    [JsonPropertyName("milestone_id")]
    public long? MilestoneId { get; set; }

    [JsonPropertyName("labels")]
    public string Labels { get; set; }

    [JsonPropertyName("squash")]
    public bool Squash { get; set; }

    [JsonPropertyName("allow_collaboration")]
    public bool? AllowCollaboration { get; set; }
}
