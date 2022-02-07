using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NGitLab.Extensions;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequest
    {
        public const string Url = "/merge_requests";

        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "iid")]
        [JsonPropertyName("iid")]
        public int Iid;

        [DataMember(Name = "state")]
        [JsonPropertyName("state")]
        public string State;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "assignee")]
        [JsonPropertyName("assignee")]
        public User Assignee;

        [DataMember(Name = "author")]
        [JsonPropertyName("author")]
        public User Author;

        [DataMember(Name = "created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "downvotes")]
        [JsonPropertyName("downvotes")]
        public int Downvotes;

        [DataMember(Name = "upvotes")]
        [JsonPropertyName("upvotes")]
        public int Upvotes;

        [DataMember(Name = "updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "target_branch")]
        [JsonPropertyName("target_branch")]
        public string TargetBranch;

        [DataMember(Name = "source_branch")]
        [JsonPropertyName("source_branch")]
        public string SourceBranch;

        [DataMember(Name = "project_id")]
        [JsonPropertyName("project_id")]
        public int ProjectId;

        [DataMember(Name = "source_project_id")]
        [JsonPropertyName("source_project_id")]
        public int SourceProjectId;

        [DataMember(Name = "target_project_id")]
        [JsonPropertyName("target_project_id")]
        public int TargetProjectId;

        [DataMember(Name = "work_in_progress")]
        [JsonPropertyName("work_in_progress")]
        public bool? WorkInProgress;

        [DataMember(Name = "milestone")]
        [JsonPropertyName("milestone")]
        public Milestone Milestone;

        [DataMember(Name = "labels")]
        [JsonPropertyName("labels")]
        public string[] Labels;

        [DataMember(Name = "merge_when_pipeline_succeeds")]
        [JsonPropertyName("merge_when_pipeline_succeeds")]
        public bool MergeWhenPipelineSucceeds;

        [DataMember(Name = "merge_status")]
        [JsonPropertyName("merge_status")]
        public string MergeStatus;

        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public string Sha;

        [DataMember(Name = "merge_commit_sha")]
        [JsonPropertyName("merge_commit_sha")]
        public string MergeCommitSha;

        [DataMember(Name = "squash_commit_sha")]
        [JsonPropertyName("squash_commit_sha")]
        public string SquashCommitSha;

        [DataMember(Name = "diff_refs")]
        [JsonPropertyName("diff_refs")]
        public DiffRefs DiffRefs;

        [DataMember(Name = "should_remove_source_branch")]
        [JsonPropertyName("should_remove_source_branch")]
        public bool? ShouldRemoveSourceBranch;

        [DataMember(Name = "force_remove_source_branch")]
        [JsonPropertyName("force_remove_source_branch")]
        public bool ForceRemoveSourceBranch;

        [DataMember(Name = "squash")]
        [JsonPropertyName("squash")]
        public bool Squash;

        [DataMember(Name = "changes_count")]
        [JsonPropertyName("changes_count")]
        public string ChangesCount;

        [DataMember(Name = "web_url")]
        [JsonPropertyName("web_url")]
        public string WebUrl;

        [DataMember(Name = "merged_by")]
        [JsonPropertyName("merged_by")]
        public User MergedBy;

        [DataMember(Name = "merged_at")]
        [JsonPropertyName("merged_at")]
        public DateTime? MergedAt;

        [DataMember(Name = "closed_at")]
        [JsonPropertyName("closed_at")]
        public DateTime? ClosedAt;

        [DataMember(Name = "closed_by")]
        [JsonPropertyName("closed_by")]
        public User ClosedBy;

        [DataMember(Name = "assignees")]
        [JsonPropertyName("assignees")]
        public User[] Assignees;

        [DataMember(Name = "reviewers")]
        [JsonPropertyName("reviewers")]
        public User[] Reviewers;

        [DataMember(Name = "allow_collaboration")]
        [JsonPropertyName("allow_collaboration")]
        public bool? AllowCollaboration;

        [DataMember(Name = "head_pipeline")]
        [JsonPropertyName("head_pipeline")]
        public Pipeline HeadPipeline;

        [DataMember(Name = "rebase_in_progress")]
        [JsonPropertyName("rebase_in_progress")]
        public bool RebaseInProgress;

        [DataMember(Name = "has_conflicts")]
        [JsonPropertyName("has_conflicts")]
        public bool HasConflicts { get; set; }

        [DataMember(Name = "user")]
        [JsonPropertyName("user")]
        public MergeRequestUserInfo User { get; set; }

        public override string ToString()
        {
            return $"!{Id.ToStringInvariant()}: {Title}";
        }
    }
}
