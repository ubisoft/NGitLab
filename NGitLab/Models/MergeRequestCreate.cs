using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MergeRequestCreate
{
    [JsonPropertyName("source_branch")]
    public string SourceBranch;

    [JsonPropertyName("target_branch")]
    public string TargetBranch;

    [JsonPropertyName("assignee_id")]
    public long? AssigneeId;

    [JsonPropertyName("assignee_ids")]
    public long[] AssigneeIds;

    [JsonPropertyName("reviewer_ids")]
    public long[] ReviewerIds;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("target_project_id")]
    public long? TargetProjectId;

    [JsonPropertyName("remove_source_branch")]
    public bool RemoveSourceBranch;

    [JsonPropertyName("milestone_id")]
    public long? MilestoneId { get; set; }

    [JsonPropertyName("labels")]
    public string Labels;

    [JsonPropertyName("squash")]
    public bool Squash;

    [JsonPropertyName("allow_collaboration")]
    public bool? AllowCollaboration;
}
