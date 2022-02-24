using System;
using System.Linq;
using LibGit2Sharp;

namespace NGitLab.Mock.Clients
{
    public static class LibGit2SharpExtensions
    {
        public static Models.Commit ToCommitClient(this LibGit2Sharp.Commit commit, Project project)
        {
            var commitInfo = project.CommitInfos.SingleOrDefault(c => string.Equals(c.Sha, commit.Sha, StringComparison.Ordinal));
            return new Models.Commit
            {
                AuthoredDate = commit.Author.When.UtcDateTime,
                AuthorEmail = commit.Author.Email,
                AuthorName = commit.Author.Name,
                CommittedDate = commit.Committer.When.UtcDateTime,
                CommitterEmail = commit.Committer.Email,
                CommitterName = commit.Committer.Name,
                CreatedAt = commit.Committer.When.UtcDateTime,
                Id = new Sha1(commit.Sha),
                Message = commit.Message,
                ShortId = commit.Sha.Substring(0, 8),
                Title = commit.MessageShort,
                Parents = commit.Parents.Select(p => new Sha1(p.Sha)).ToArray(),
                Status = commitInfo?.Status ?? "success",
                WebUrl = $"{project.WebUrl}/-/commits/{commit.Sha}",
            };
        }

        public static Models.Branch ToBranchClient(this LibGit2Sharp.Branch branch, Project project)
        {
            var commit = branch.Tip;
            return new Models.Branch
            {
                CanPush = true,
                Protected = false,
                DevelopersCanMerge = true,
                DevelopersCanPush = true,
                Merged = false,
                Name = branch.FriendlyName,
                Default = string.Equals(branch.FriendlyName, project.DefaultBranch, System.StringComparison.Ordinal),
                Commit = ToCommitInfo(commit),
            };
        }

        public static Models.CommitInfo ToCommitInfo(this LibGit2Sharp.Commit commit)
        {
            return new Models.CommitInfo
            {
                Id = new Sha1(commit.Sha),
                AuthorName = commit.Author.Name,
                AuthorEmail = commit.Author.Email,
                AuthoredDate = commit.Author.When.UtcDateTime,
                CommitterName = commit.Committer.Name,
                CommitterEmail = commit.Committer.Email,
                CommittedDate = commit.Committer.When.UtcDateTime,
                Message = commit.Message,
                Parents = commit.Parents.Select(c => new Sha1(c.Sha)).ToArray(),
            };
        }

        internal static Commit GetLastCommitForFileChanges(this LibGit2Sharp.Repository repository, string filePath)
        {
            return repository.Commits.QueryBy(filePath).FirstOrDefault()?.Commit;
        }
    }
}
