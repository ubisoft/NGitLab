using System;
using System.Text.Json.Serialization;
using NGitLab.Extensions;

namespace NGitLab.Models;

public class MergeRequest
{
    public const string Url = "/merge_requests";

    [JsonPropertyName("id")]
    public int Id;

    [JsonPropertyName("iid")]
    public int Iid;

    [JsonPropertyName("state")]
    public string State;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("assignee")]
    public User Assignee;

    [JsonPropertyName("author")]
    public User Author;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("user_notes_count")]
    public int UserNotesCount;

    [JsonPropertyName("downvotes")]
    public int Downvotes;

    [JsonPropertyName("upvotes")]
    public int Upvotes;

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt;

    [JsonPropertyName("target_branch")]
    public string TargetBranch;

    [JsonPropertyName("source_branch")]
    public string SourceBranch;

    [JsonPropertyName("project_id")]
    public int ProjectId;

    [JsonPropertyName("source_project_id")]
    public int SourceProjectId;

    [JsonPropertyName("target_project_id")]
    public int TargetProjectId;

    [JsonPropertyName("work_in_progress")]
    public bool? WorkInProgress;

    [JsonPropertyName("milestone")]
    public Milestone Milestone;

    [JsonPropertyName("labels")]
    public string[] Labels;

    [JsonPropertyName("merge_when_pipeline_succeeds")]
    public bool MergeWhenPipelineSucceeds;

    [JsonPropertyName("merge_status")]
    public string MergeStatus;

    [JsonPropertyName("sha")]
    public string Sha;

    [JsonPropertyName("merge_commit_sha")]
    public string MergeCommitSha;

    [JsonPropertyName("squash_commit_sha")]
    public string SquashCommitSha;

    [JsonPropertyName("diff_refs")]
    public DiffRefs DiffRefs;

    [JsonPropertyName("should_remove_source_branch")]
    public bool? ShouldRemoveSourceBranch;

    [JsonPropertyName("force_remove_source_branch")]
    public bool ForceRemoveSourceBranch;

    [JsonPropertyName("squash")]
    public bool Squash;

    [JsonPropertyName("changes_count")]
    public string ChangesCount;

    [JsonPropertyName("web_url")]
    public string WebUrl;

    [JsonPropertyName("merged_by")]
    public User MergedBy;

    [JsonPropertyName("merged_at")]
    public DateTime? MergedAt;

    [JsonPropertyName("closed_at")]
    public DateTime? ClosedAt;

    [JsonPropertyName("closed_by")]
    public User ClosedBy;

    [JsonPropertyName("assignees")]
    public User[] Assignees;

    [JsonPropertyName("reviewers")]
    public User[] Reviewers;

    [JsonPropertyName("allow_collaboration")]
    public bool? AllowCollaboration;

    [JsonPropertyName("head_pipeline")]
    public Pipeline HeadPipeline;

    [JsonPropertyName("rebase_in_progress")]
    public bool RebaseInProgress;

    [JsonPropertyName("diverged_commits_count")]
    public int? DivergedCommitsCount { get; set; }

    [JsonPropertyName("has_conflicts")]
    public bool HasConflicts { get; set; }

    [JsonPropertyName("blocking_discussions_resolved")]
    public bool BlockingDiscussionsResolved { get; set; }

    [JsonPropertyName("user")]
    public MergeRequestUserInfo User { get; set; }

    [JsonPropertyName("detailed_merge_status")]
    public DynamicEnum<DetailedMergeStatus> DetailedMergeStatus { get; set; }

    public override string ToString() => $"!{Id.ToStringInvariant()}: {Title}";
}
