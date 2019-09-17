using System;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class MergeRequest : GitLabObject
    {
        public MergeRequest()
        {
            Comments = new NoteCollection<MergeRequestComment>(this);
        }

        public Project Project => (Project)Parent;

        public int Id { get; internal set; }
        public int Iid { get; internal set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserRef Author { get; set; }
        public UserRef Assignee { get; set; }
        public string SourceBranch { get; set; }
        public string TargetBranch { get; set; }
        public Project SourceProject { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? MergedAt { get; set; }
        public DateTimeOffset? ClosedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Sha1 Sha { get; set; }
        public Sha1? MergeCommitSha { get; set; }
        public bool ShouldRemoveSourceBranch { get; set; }
        public bool ForceRemoveSourceBranch { get; set; }
        public bool Squash { get; set; }
        public bool MergeWhenPipelineSucceeds { get; set; }
        public NoteCollection<MergeRequestComment> Comments { get; }

        public bool WorkInProgress => Title?.StartsWith("WIP:", StringComparison.OrdinalIgnoreCase) == true;

        public MergeRequestState State
        {
            get
            {
                if (MergedAt.HasValue)
                    return MergeRequestState.merged;

                if (ClosedAt.HasValue)
                    return MergeRequestState.closed;

                return MergeRequestState.opened;
            }
        }

        internal Models.MergeRequest ToMergeRequestClient()
        {
            return new Models.MergeRequest
            {
                Assignee = Assignee?.ToUserClient(),
                Author = Author.ToUserClient(),
                CreatedAt = CreatedAt.UtcDateTime,
                UpdatedAt = UpdatedAt.UtcDateTime,
                Description = Description,
                Id = Id,
                Iid = Iid,
                ProjectId = Project.Id,
                MergeCommitSha = MergeCommitSha?.ToString(),
                Sha = Sha.ToString(),
                ForceRemoveSourceBranch = ForceRemoveSourceBranch,
                MergeWhenPipelineSucceeds = MergeWhenPipelineSucceeds,
                ShouldRemoveSourceBranch = ShouldRemoveSourceBranch,
                SourceBranch = SourceBranch,
                SourceProjectId = SourceProject.Id,
                TargetBranch = TargetBranch,
                TargetProjectId = Project.Id,
                Title = Title,
                WorkInProgress = WorkInProgress,
                Squash = Squash,
                MergeStatus = "can_be_merged",
                State = State.ToString(),
            };
        }
    }
}
