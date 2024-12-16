using System;
using System.Text.Json.Serialization;
using NGitLab.Extensions;

namespace NGitLab.Models;

public class MergeRequest
{
    public const string Url = "/merge_requests";

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("iid")]
    public long Iid { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("assignee")]
    public User Assignee { get; set; }

    [JsonPropertyName("author")]
    public User Author { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("user_notes_count")]
    public int UserNotesCount { get; set; }

    [JsonPropertyName("downvotes")]
    public int Downvotes { get; set; }

    [JsonPropertyName("draft")]
    public bool Draft { get; set; }

    [JsonPropertyName("upvotes")]
    public int Upvotes { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("target_branch")]
    public string TargetBranch { get; set; }

    [JsonPropertyName("source_branch")]
    public string SourceBranch { get; set; }

    [JsonPropertyName("project_id")]
    public long ProjectId { get; set; }

    [JsonPropertyName("source_project_id")]
    public long SourceProjectId { get; set; }

    [JsonPropertyName("target_project_id")]
    public long TargetProjectId { get; set; }

    [Obsolete("Deprecated by GitLab. Use Draft instead")]
    [JsonPropertyName("work_in_progress")]
    public bool? WorkInProgress { get; set; }

    [JsonPropertyName("milestone")]
    public Milestone Milestone { get; set; }

    [JsonPropertyName("labels")]
    public string[] Labels { get; set; }

    [JsonPropertyName("merge_when_pipeline_succeeds")]
    public bool MergeWhenPipelineSucceeds { get; set; }

    [JsonPropertyName("merge_status")]
    public string MergeStatus { get; set; }

    [JsonPropertyName("sha")]
    public string Sha { get; set; }

    [JsonPropertyName("merge_commit_sha")]
    public string MergeCommitSha { get; set; }

    [JsonPropertyName("squash_commit_sha")]
    public string SquashCommitSha { get; set; }

    [JsonPropertyName("diff_refs")]
    public DiffRefs DiffRefs { get; set; }

    [JsonPropertyName("should_remove_source_branch")]
    public bool? ShouldRemoveSourceBranch { get; set; }

    [JsonPropertyName("force_remove_source_branch")]
    public bool ForceRemoveSourceBranch { get; set; }

    [JsonPropertyName("squash")]
    public bool Squash { get; set; }

    [JsonPropertyName("changes_count")]
    public string ChangesCount { get; set; }

    [JsonPropertyName("web_url")]
    public string WebUrl { get; set; }

    [JsonPropertyName("merged_by")]
    public User MergedBy { get; set; }

    [JsonPropertyName("merged_at")]
    public DateTime? MergedAt { get; set; }

    [JsonPropertyName("closed_at")]
    public DateTime? ClosedAt { get; set; }

    [JsonPropertyName("closed_by")]
    public User ClosedBy { get; set; }

    [JsonPropertyName("assignees")]
    public User[] Assignees { get; set; }

    [JsonPropertyName("reviewers")]
    public User[] Reviewers { get; set; }

    [JsonPropertyName("allow_collaboration")]
    public bool? AllowCollaboration { get; set; }

    [JsonPropertyName("head_pipeline")]
    public Pipeline HeadPipeline { get; set; }

    [JsonPropertyName("rebase_in_progress")]
    public bool RebaseInProgress { get; set; }

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

    [JsonPropertyName("merge_error")]
    public string MergeError { get; set; }

    public override string ToString() => $"!{Id.ToStringInvariant()}: {Title}";
}
