using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class MergeRequest : GitLabObject
    {
        private string _consolidatedSourceBranch;

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

        public UserRef Assignee
        {
            get => Assignees?.FirstOrDefault();
            set
            {
                Assignees.Clear();
                if (value != null)
                {
                    Assignees.Add(value);
                }
            }
        }

        public IList<UserRef> Assignees { get; set; } = new List<UserRef>();

        public IList<UserRef> Reviewers { get; set; } = new List<UserRef>();

        public string SourceBranch { get; set; }

        public string TargetBranch { get; set; }

        public Project SourceProject { get; set; }

        public bool HasConflicts { get; set; }

        public string HeadSha { get; set; }

        public string StartSha { get; set; }

        public string BaseSha { get; set; }

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

        public string WebUrl => Server.MakeUrl($"{Project.PathWithNamespace}/-/merge_requests/{Iid.ToString(CultureInfo.InvariantCulture)}");

        public Pipeline HeadPipeline => Project.Pipelines
                  .Where(p => p.Sha.Equals(Sha))
                  .OrderByDescending(p => p.CreatedAt)
                  .FirstOrDefault();

        public IList<string> Labels { get; } = new List<string>();

        public NoteCollection<MergeRequestComment> Comments { get; }

        public bool WorkInProgress => Title is not null &&
            (Title.StartsWith("WIP:", StringComparison.OrdinalIgnoreCase) ||
             Title.StartsWith("Draft:", StringComparison.OrdinalIgnoreCase));

        public IList<UserRef> Approvers { get; } = new List<UserRef>();

        public MergeRequestChangeCollection Changes
        {
            get
            {
                var stats = Project.Repository.GetBranchFullPatch(SourceBranch);
                var changes = new MergeRequestChangeCollection(this);
                foreach (var stat in stats)
                {
                    var diff = stat.Patch.Substring(stat.Patch.IndexOf("@@", StringComparison.Ordinal));
                    changes.Add(diff, stat.OldPath, stat.Path, stat.Status);
                }

                return changes;
            }
        }

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
            var mergeCommit = Project.Repository.Merge(user, ConsolidatedSourceBranch, TargetBranch, Project);

            MergeCommitSha = new Sha1(mergeCommit.Sha);
            MergedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;

            if (ForceRemoveSourceBranch || ShouldRemoveSourceBranch)
            {
                SourceProject.Repository.RemoveBranch(SourceBranch);
            }

            if (Project != SourceProject)
                Project.Repository.RemoveBranch(ConsolidatedSourceBranch);
        }

        public RebaseResult Rebase(User user)
        {
            SourceProject.Repository.Rebase(user, SourceBranch, TargetBranch);

            UpdatedAt = DateTimeOffset.UtcNow;

            return new RebaseResult { RebaseInProgress = true };
        }

        public IEnumerable<MergeRequestDiscussion> GetDiscussions()
        {
            var sha1 = SHA1.Create();
            var i = 0;
            var discussions = new List<MergeRequestDiscussion>();
            foreach (var comment in Comments)
            {
                if (string.IsNullOrEmpty(comment.ThreadId))
                {
                    discussions.Add(new MergeRequestDiscussion
                    {
                        Id = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes($"{Iid}:{i}"))),
                        IndividualNote = true,
                        Notes = new[] { comment.ToMergeRequestCommentClient() },
                    });
                }
                else
                {
                    var discussion = discussions.Find(x => string.Equals(x.Id, comment.ThreadId, StringComparison.Ordinal));
                    if (discussion == null)
                    {
                        discussions.Add(new MergeRequestDiscussion
                        {
                            Id = comment.ThreadId,
                            IndividualNote = false,
                            Notes = new[] { comment.ToMergeRequestCommentClient() },
                        });
                    }
                    else
                    {
                        discussion.Notes = discussion.Notes.Concat(new[] { comment.ToMergeRequestCommentClient() }).ToArray();
                    }
                }

                i++;
            }

            return discussions;
        }

        internal Models.MergeRequest ToMergeRequestClient()
        {
            return new Models.MergeRequest
            {
                Assignees = GetUsers(Assignees),
                Author = Author.ToUserClient(),
                CreatedAt = CreatedAt.UtcDateTime,
                UpdatedAt = UpdatedAt.UtcDateTime,
                MergedAt = MergedAt?.UtcDateTime,
                ClosedAt = ClosedAt?.UtcDateTime,
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
                Reviewers = GetUsers(Reviewers),
                HasConflicts = HasConflicts,
                DiffRefs = new DiffRefs
                {
                    BaseSha = BaseSha,
                    StartSha = StartSha,
                    HeadSha = HeadSha,
                },
            };
        }

        internal static Models.User[] GetUsers(IList<UserRef> userRefs)
        {
            var users = new List<Models.User>();
            foreach (var userRef in userRefs)
            {
                var user = userRef.ToUserClient();
                users.Add(user);
            }

            return users.ToArray();
        }

        internal string ConsolidatedSourceBranch
        {
            get
            {
                _consolidatedSourceBranch ??=
                    SourceProject == Project ?
                    SourceBranch :
                    Project.Repository.FetchBranchFromFork(SourceProject, SourceBranch);

                return _consolidatedSourceBranch;
            }
        }
    }
}
