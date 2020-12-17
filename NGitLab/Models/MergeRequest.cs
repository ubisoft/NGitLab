using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequest
    {
        public const string Url = "/merge_requests";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "iid")]
        public int Iid;

        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "assignee")]
        public User Assignee;

        [DataMember(Name = "author")]
        public User Author;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "downvotes")]
        public int Downvotes;

        [DataMember(Name = "upvotes")]
        public int Upvotes;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "target_branch")]
        public string TargetBranch;

        [DataMember(Name = "source_branch")]
        public string SourceBranch;

        [DataMember(Name = "project_id")]
        public int ProjectId;

        [DataMember(Name = "source_project_id")]
        public int SourceProjectId;

        [DataMember(Name = "target_project_id")]
        public int TargetProjectId;

        [DataMember(Name = "work_in_progress")]
        public bool? WorkInProgress;

        [DataMember(Name = "milestone")]
        public Milestone Milestone;

        [DataMember(Name = "labels")]
        public string[] Labels;

        [DataMember(Name = "merge_when_pipeline_succeeds")]
        public bool MergeWhenPipelineSucceeds;

        [DataMember(Name = "merge_status")]
        public string MergeStatus;

        [DataMember(Name = "sha")]
        public string Sha;

        [DataMember(Name = "merge_commit_sha")]
        public string MergeCommitSha;

        [DataMember(Name = "should_remove_source_branch")]
        public bool? ShouldRemoveSourceBranch;

        [DataMember(Name = "force_remove_source_branch")]
        public bool ForceRemoveSourceBranch;

        [DataMember(Name = "squash")]
        public bool Squash;

        [DataMember(Name = "web_url")]
        public string WebUrl;

        [DataMember(Name = "merged_by")]
        public User MergedBy;

        [DataMember(Name = "merged_at")]
        public DateTime? MergedAt;

        [DataMember(Name = "closed_at")]
        public DateTime? ClosedAt;

        [DataMember(Name = "closed_by")]
        public User ClosedBy;

        [DataMember(Name = "assignees")]
        public User[] Assignees;

        [DataMember(Name = "allow_collaboration")]
        public bool? AllowCollaboration;

        [DataMember(Name = "head_pipeline")]
        public Pipeline HeadPipeline;

        [DataMember(Name = "rebase_in_progress")]
        public bool RebaseInProgress;

        [DataMember(Name = "has_conflicts")]
        public bool HasConflicts { get; set; }

        [DataMember(Name = "user")]
        public MergeRequestUserInfo User { get; set; }

        public override string ToString()
        {
            return $"!{Id}: {Title}";
        }
    }
}
