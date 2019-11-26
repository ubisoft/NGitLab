using LibGit2Sharp;
using System.Linq;

namespace NGitLab.Mock.Clients
{
    public static class LibGit2SharpExtensions
    {
        public static Models.Commit ToCommitClient(this LibGit2Sharp.Commit commit)
        {
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
                Default = branch.FriendlyName == project.DefaultBranch,
                Commit = new Models.CommitInfo
                {
                    Author = new Models.PersonInfo
                    {
                        Email = commit.Author.Email,
                        Name = commit.Author.Name,
                    },
                    Committer = new Models.PersonInfo
                    {
                        Email = commit.Committer.Email,
                        Name = commit.Committer.Name,
                    },
                    AuthoredDate = commit.Author.When.UtcDateTime,
                    CommittedDate = commit.Committer.When.UtcDateTime,
                    Id = new Sha1(commit.Sha),
                    Message = commit.Message,
                    Tree = new Sha1(commit.Tree.Sha),
                    Parents = commit.Parents.Select(p => new Sha1(p.Sha)).ToArray(),
                },
            };
        }

        public static Models.CommitInfo ToCommitInfo(this LibGit2Sharp.Commit commit)
        {
            return new Models.CommitInfo
            {
                Id = new Sha1(commit.Sha),
                Author = new Models.PersonInfo
                {
                    Name = commit.Author.Name,
                    Email = commit.Author.Email,
                },
                AuthoredDate = commit.Author.When.UtcDateTime,
                Committer = new Models.PersonInfo
                {
                    Name = commit.Committer.Name,
                    Email = commit.Committer.Email,
                },
                CommittedDate = commit.Committer.When.UtcDateTime,
                Message = commit.Message,
                Parents = commit.Parents.Select(c => new Sha1(c.Sha)).ToArray(),
                Tree = new Sha1(commit.Tree.Sha),
            };
        }

        internal static Commit GetLastCommitForFileChanges(this LibGit2Sharp.Repository repository, string filePath)
        {
            return repository.Commits.QueryBy(filePath).FirstOrDefault()?.Commit;
        }
    }
}
