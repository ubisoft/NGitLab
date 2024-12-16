using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Issue
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("iid")]
    public long IssueId { get; set; }

    [JsonPropertyName("project_id")]
    public long ProjectId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("labels")]
    public string[] Labels { get; set; }

    [JsonPropertyName("milestone")]
    public Milestone Milestone { get; set; }

    [JsonPropertyName("assignee")]
    public Assignee Assignee { get; set; }

    [JsonPropertyName("assignees")]
    public Assignee[] Assignees { get; set; }

    [JsonPropertyName("author")]
    public Author Author { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("closed_at")]
    public DateTime ClosedAt { get; set; }

    [JsonPropertyName("closed_by")]
    public User ClosedBy { get; set; }

    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("merge_requests_count")]
    public int MergeRequestsCount { get; set; }

    [JsonPropertyName("epic")]
    public IssueEpic Epic { get; set; }

    [JsonPropertyName("confidential")]
    public bool Confidential { get; set; }

    [JsonPropertyName("weight")]
    public int? Weight { get; set; }

    [JsonPropertyName("issue_type")]
    public string IssueType { get; set; }

    [JsonPropertyName("moved_to_id")]
    public long? MovedToId { get; set; }

    [JsonPropertyName("references")]
    public References References { get; set; }
}
