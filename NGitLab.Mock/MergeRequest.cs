using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? MergedAt { get; set; }

        public DateTimeOffset? ClosedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Sha1 Sha { get; private set; }

        public Sha1? MergeCommitSha { get; set; }

        public bool ShouldRemoveSourceBranch { get; set; }

        public bool ForceRemoveSourceBranch { get; set; }

        public bool Squash { get; set; }

        public bool MergeWhenPipelineSucceeds { get; set; }

        public bool RebaseInProgress { get; set; }

        public string WebUrl => Server.MakeUrl($"{Project.PathWithNamespace}/merge_requests/{Id.ToString(CultureInfo.InvariantCulture)}");

        public Pipeline HeadPipeline
        {
            get
            {
                return Project.Pipelines
                  .Where(p => p.Sha.Equals(Sha))
                  .OrderByDescending(p => p.CreatedAt)
                  .FirstOrDefault();
            }
        }

        public IList<string> Labels { get; } = new List<string>();

        public NoteCollection<MergeRequestComment> Comments { get; }

        public bool WorkInProgress => Title?.StartsWith("WIP:", StringComparison.OrdinalIgnoreCase) == true;

        public IList<UserRef> Approvers { get; } = new List<UserRef>();

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

        public IEnumerable<LibGit2Sharp.Commit> Commits
        {
            get
            {
                var targetBranch = Project.Repository.GetBranch(TargetBranch);
                return SourceProject.Repository.GetBranchCommits(SourceBranch).TakeWhile(commit => commit.Id != targetBranch.Tip.Id);
            }
        }

        internal void UpdateSha()
        {
            var commit = SourceProject.Repository.GetBranchTipCommit(SourceBranch);
            if (commit != null)
            {
                Sha = new Sha1(commit.Sha);
            }
        }

        public void Accept(User user)
        {
            var mergeCommit = SourceProject.Repository.Merge(user, SourceBranch, TargetBranch, Project);

            MergeCommitSha = new Sha1(mergeCommit.Sha);
            MergedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;

            if (ForceRemoveSourceBranch || ShouldRemoveSourceBranch)
            {
                SourceProject.Repository.RemoveBranch(SourceBranch);
            }
        }

        public RebaseResult Rebase(User user)
        {
            SourceProject.Repository.Rebase(user, SourceBranch, TargetBranch, Project);

            UpdatedAt = DateTimeOffset.UtcNow;

            return new RebaseResult { RebaseInProgress = true };
        }

        public IEnumerable<Models.MergeRequestDiscussion> GetDiscussions()
        {
            return Comments.GroupBy(c => c.ThreadId, StringComparer.Ordinal).Select(g => new MergeRequestDiscussion
            {
                Id = g.Key,
                IndividualNote = g.Count() == 1,
                Notes = g.Select(n => n.ToMergeRequestCommentClient()).ToArray(),
            });
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
                WebUrl = WebUrl,
                HeadPipeline = HeadPipeline?.ToPipelineClient(),
                Labels = Labels.ToArray(),
                RebaseInProgress = RebaseInProgress,
            };
        }
    }
}
