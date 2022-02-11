using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Issue
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "iid")]
        [JsonPropertyName("iid")]
        public int IssueId;

        [DataMember(Name = "project_id")]
        [JsonPropertyName("project_id")]
        public int ProjectId;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string[] Labels;

        [DataMember(Name = "milestone")]
        [JsonPropertyName("milestone")]
        public Milestone Milestone;

        [DataMember(Name = "assignee")]
        [JsonPropertyName("assignee")]
        public Assignee Assignee;

        [DataMember(Name = "assignees")]
        [JsonPropertyName("assignees")]
        public Assignee[] Assignees;

        [DataMember(Name = "author")]
        [JsonPropertyName("author")]
        public Author Author;

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "due_date")]
        [JsonPropertyName("due_date")]
        public DateTime? DueDate;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;

        [DataMember(Name = "merge_requests_count")]
        [JsonPropertyName("merge_requests_count")]
        public int MergeRequestsCount { get; set; }

        [DataMember(Name = "epic")]
        [JsonPropertyName("epic")]
        public IssueEpic Epic;

        [DataMember(Name = "confidential")]
        [JsonPropertyName("confidential")]
        public bool Confidential;

        [DataMember(Name = "weight")]
        [JsonPropertyName("weight")]
        public int? Weight { get; set; }
    }
}
