using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Issue
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("iid")]
        public int IssueId;

        [JsonPropertyName("project_id")]
        public int ProjectId;

        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("labels")]
        public string[] Labels;

        [JsonPropertyName("milestone")]
        public Milestone Milestone;

        [JsonPropertyName("assignee")]
        public Assignee Assignee;

        [JsonPropertyName("assignees")]
        public Assignee[] Assignees;

        [JsonPropertyName("author")]
        public Author Author;

        [JsonPropertyName("state")]
        public string State;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [JsonPropertyName("closed_at")]
        public DateTime ClosedAt { get; set; }

        [JsonPropertyName("closed_by")]
        public User ClosedBy { get; set; }

        [JsonPropertyName("due_date")]
        public DateTime? DueDate;

        [JsonPropertyName("web_url")]
        public string WebUrl;

        [JsonPropertyName("merge_requests_count")]
        public int MergeRequestsCount { get; set; }

        [JsonPropertyName("epic")]
        public IssueEpic Epic;

        [JsonPropertyName("confidential")]
        public bool Confidential;

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }
    }
}
