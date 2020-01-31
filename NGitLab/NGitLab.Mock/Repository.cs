using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LibGit2Sharp;
using NGitLab.Mock.Clients;
using Internals.FileUtilities;
using Internals.Abstractions;

namespace NGitLab.Mock
{
    public sealed class Repository : GitLabObject, IDisposable
    {
        private static int _tempBranchName = 0;

        private readonly object _lock = new object();
        private TemporaryDirectory _directory;
        private LibGit2Sharp.Repository _repository;
        private readonly IList<ReleaseTag> _releaseTags = new List<ReleaseTag>();

        public Project Project => (Project)Parent;

        internal Repository(Project project)
        {
            base.Parent = project;
        }

        public string FullPath
        {
            get
            {
                if (_directory == null)
                {
                    _ = GetGitRepository();
                }

                return _directory.FullPath;
            }
        }

        private LibGit2Sharp.Repository GetGitRepository()
        {
            if (_directory == null)
            {
                lock (_lock)
                {
                    if (_directory == null)
                    {
                        var directory = TemporaryDirectory.Create();
                        if (Project.ForkedFrom == null)
                        {
                            LibGit2Sharp.Repository.Init(directory.FullPath);
                        }
                        else
                        {
                            LibGit2Sharp.Repository.Clone(Project.ForkedFrom.Repository.FullPath, directory.FullPath);
                        }

                        _repository = new LibGit2Sharp.Repository(directory.FullPath);
                        _directory = directory;
                    }
                }
            }

            return _repository;
        }

        public Branch GetBranch(string branchName)
        {
            var repository = GetGitRepository();
            return repository.Branches[branchName];
        }

        public IReadOnlyCollection<Branch> GetAllBranches()
        {
            var repository = GetGitRepository();
            return repository.Branches.ToList();
        }

        public Commit Commit(User user, string message)
        {
            return Commit(user, message, null, new[] { File.CreateFromText("test.txt", Guid.NewGuid().ToString()) });
        }

        public Commit Commit(User user, string message, IEnumerable<File> files)
        {
            return Commit(user, message, null, files);
        }

        public Commit Commit(User user, string message, string targetBranch, IEnumerable<File> files)
        {
            var repository = GetGitRepository();
            if (targetBranch != null)
            {
                Commands.Checkout(repository, targetBranch);
            }

            foreach (var file in files)
            {
                var fullPath = Path.Combine(FullPath, file.Path);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                System.IO.File.WriteAllBytes(fullPath, file.Content);
                repository.Index.Add(file.Path);
            }

            repository.Index.Write();

            var author = new Signature(user.UserName, user.Email, DateTimeOffset.UtcNow);
            var committer = author;
            return repository.Commit(message, author, committer);
        }

        public Commit Commit(Models.CommitCreate commitCreate)
        {
            var repo = GetGitRepository();
            bool mustCreateBranch = !repo.Commits.Any();
            if (!mustCreateBranch)
            {
                Commands.Checkout(repo, commitCreate.Branch);
            }

            var commit = CreateOnNonBareRepo(commitCreate);
            bool branchStillMissing = !repo.Branches.Any(b => string.Equals(b.FriendlyName, commitCreate.Branch, StringComparison.Ordinal));
            if (mustCreateBranch && branchStillMissing)
            {
                repo.Branches.Add(commitCreate.Branch, commit.Sha);
            }

            return commit;
        }

        public void Checkout(string committishOrBranchNameSpec)
        {
            Commands.Checkout(GetGitRepository(), committishOrBranchNameSpec);
        }

        public Branch CreateAndCheckoutBranch(string branchName)
        {
            var branch = CreateBranch(branchName, "HEAD");
            Checkout(branch.CanonicalName);
            return branch;
        }

        public Branch CreateBranch(string branchName)
        {
            return CreateBranch(branchName, "HEAD");
        }

        public Branch CreateBranch(string branchName, string reference)
        {
            var repository = GetGitRepository();
            return repository.Branches.Add(branchName, reference);
        }

        public void RemoveBranch(string branchName)
        {
            var repository = GetGitRepository();
            repository.Branches.Remove(branchName);
        }

        public TagCollection GetTags()
        {
            var repository = GetGitRepository();
            return repository.Tags;
        }

        public Tag CreateTag(string tagName)
        {
            return CreateTag(user: null, tagName, "HEAD", message: null, releaseNotes: null);
        }

        public Tag CreateTag(string tagName, string reference)
        {
            return CreateTag(user: null, tagName, reference, message: null, releaseNotes: null);
        }

        public Tag CreateTag(User user, string tagName, string reference, string message)
        {
            return CreateTag(user, tagName, reference, message, releaseNotes: null);
        }

        public Tag CreateTag(User user, string tagName, string reference, string message, string releaseNotes)
        {
            var repository = GetGitRepository();
            Tag tag;

            if (string.IsNullOrEmpty(message))
            {
                tag = repository.ApplyTag(tagName, reference);
            }
            else
            {
                var tagger = new Signature(user.Name, user.Email, DateTimeOffset.UtcNow);
                tag = repository.ApplyTag(tagName, reference, tagger, message);
            }

            if (!string.IsNullOrEmpty(releaseNotes))
            {
                _releaseTags.Add(new ReleaseTag(tagName, releaseNotes));
            }
            return tag;
        }

        public ReleaseTag GetReleaseTag(string tagName)
        {
            return _releaseTags.FirstOrDefault(r => string.Equals(r.Name, tagName, StringComparison.Ordinal));
        }

        public ReleaseTag CreateReleaseTag(string tagName, string releaseNotes)
        {
            if (_releaseTags.Any(r => string.Equals(r.Name, tagName, StringComparison.Ordinal)))
            {
                throw new GitLabBadRequestException();
            }

            var releaseTag = new ReleaseTag(tagName, releaseNotes);
            _releaseTags.Add(releaseTag);
            return releaseTag;
        }

        public ReleaseTag UpdateReleaseTag(string tagName, string releaseNotes)
        {
            if (!_releaseTags.Any(r => string.Equals(r.Name, tagName, StringComparison.Ordinal)))
            {
                throw new GitLabBadRequestException();
            }

            var releaseTag = _releaseTags.First(r => string.Equals(r.Name, tagName, StringComparison.Ordinal));
            releaseTag.ReleaseNotes = releaseNotes;
            return releaseTag;
        }

        public void DeleteTag(string tagName)
        {
            var repository = GetGitRepository();
            repository.Tags.Remove(tagName);
            if (_releaseTags.Any(r => string.Equals(r.Name, tagName, StringComparison.Ordinal)))
            {
                _releaseTags.Remove(_releaseTags.First(r => string.Equals(r.Name, tagName, StringComparison.Ordinal)));
            }
        }

        public Commit Merge(User user, string sourceBranch, string targetBranch)
        {
            var repository = GetGitRepository();
            Commands.Checkout(repository, targetBranch);
            var branch = repository.Branches[sourceBranch];

            var author = new Signature(user.UserName, user.Email, DateTimeOffset.UtcNow);

            var result = repository.Merge(branch, author);
            if (result.Commit != null)
                return result.Commit;

            if (result.Status == MergeStatus.UpToDate || result.Status == MergeStatus.FastForward)
                return repository.Branches[targetBranch].Tip;

            throw new GitLabException("Could not merge");
        }

        public Commit GetBranchTipCommit(string branchName)
        {
            var branch = GetBranch(branchName);
            if (branch != null)
                return branch.Tip;

            return null;
        }

        public IEnumerable<Commit> GetBranchCommits(string branchName)
        {
            var branch = GetBranch(branchName);
            if (branch == null)
                yield break;

            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Topological,
                IncludeReachableFrom = branch,
            };

            foreach (var commit in _repository.Commits.QueryBy(filter))
            {
                yield return commit;
            }
        }

        public IEnumerable<Commit> GetCommits(string @ref)
        {
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Topological,
            };

            if (!string.IsNullOrEmpty(@ref))
            {
                filter.IncludeReachableFrom = @ref;
            }

            try
            {
                return _repository.Commits.QueryBy(filter).ToList();
            }
            catch (NotFoundException)
            {
                // @ref does not exist so no commits could be found
                return Enumerable.Empty<Commit>();
            }
        }

        public Commit GetCommit(string reference)
        {
            var repository = GetGitRepository();
            var branchTip = GetBranchTipCommit(reference);
            if (branchTip != null)
                return branchTip;

            var tag = repository.Tags[reference];
            if (tag?.PeeledTarget is Commit commit)
                return branchTip;

            return repository.Commits.SingleOrDefault(c => string.Equals(c.Sha, reference, StringComparison.Ordinal));
        }

        public Models.FileData GetFile(string filePath, string @ref)
        {
            var repo = GetGitRepository();
            Commands.Checkout(repo, @ref);
            var fileCompletePath = Path.Combine(FullPath, filePath);

            if (!System.IO.File.Exists(fileCompletePath))
            {
                throw new GitLabNotFoundException("File not found");
            }

            var fileInfo = new FileInfo(fileCompletePath);
            var commit = GetCommit(@ref);
            return new Models.FileData
            {
                Name = fileInfo.Name,
                Path = filePath,
                Size = (int)fileInfo.Length,
                Encoding = "base64",
                Content = Convert.ToBase64String(System.IO.File.ReadAllBytes(fileCompletePath)),
                Ref = @ref,
                BlobId = ((Blob)commit[filePath].Target).Id.ToString(),
                CommitId = commit.Sha,
                LastCommitId = repo.GetLastCommitForFileChanges(filePath).Sha,
            };
        }

        public Commit Merge(User user, string sourceBranch, string targetBranch, Project targetProject)
        {
            // If it's on the same project just merge branch
            if (Parent == targetProject)
                return Merge(user, sourceBranch, targetBranch);

            var repo = GetGitRepository();
            var targetGitRepository = targetProject.Repository.GetGitRepository();

            var branch = repo.Branches[sourceBranch];
            var options = new PushOptions();

            var id = Interlocked.Increment(ref _tempBranchName);
            var tempBranchName = "merge-" + id;
            var tempRemoteName = "origin-" + id;

            var remote = repo.Network.Remotes.Add(tempRemoteName, targetProject.Repository.FullPath);
            var tempBranch = repo.CreateBranch(tempBranchName, branch.Tip);
            repo.Branches.Update(tempBranch, b => b.Remote = tempRemoteName, b => b.UpstreamBranch = tempBranch.CanonicalName);

            // libgit2sharp cannot push to non-bare local repo: repo.Network.Push(remote, branch.CanonicalName, options);
            var job = new Internals.Git.GitPushJob(FullPath)
            {
                RefSpec = tempBranch.CanonicalName,
            };
            var jobResult = job.ExecuteOnDefaultContext();
            if (!jobResult.IsSuccess)
                throw new GitLabException("Cannot push branch");

            repo.Network.Remotes.Remove(tempRemoteName);
            repo.Branches.Remove(tempBranch);

            var merger = new Signature(user.UserName, user.Email, DateTimeOffset.UtcNow);
            Commands.Checkout(targetGitRepository, targetBranch);
            var result = targetGitRepository.Merge(tempBranchName, merger);
            targetGitRepository.Branches.Remove(tempBranchName);

            return result.Commit;
        }

        public void Dispose()
        {
            try
            {
                _repository?.Dispose();
                _directory?.Dispose();
            }
            catch
            {
                // It's not important if the directory is not deleted
            }
        }

        private Commit CreateOnNonBareRepo(Models.CommitCreate commit)
        {
            var repo = GetGitRepository();
            ApplyActions(commit);

            var author = new Signature(commit.AuthorName, commit.AuthorEmail, DateTimeOffset.UtcNow);
            var committer = author;

            var newCommit = repo.Commit(commit.CommitMessage, author, committer);
            return newCommit;
        }

        private void UpdateMergeRequests()
        {
            foreach (var mergeRequest in Project.MergeRequests)
            {
                mergeRequest.UpdateSha();
            }
        }

        private void ApplyActions(Models.CommitCreate commit)
        {
            var repo = GetGitRepository();

            foreach (var action in commit.Actions)
            {
                GetActionHandler(action.Action).Handle(action, FullPath, repo);
            }
        }

        private static ICommitActionHandler GetActionHandler(string action)
        {
            if (!Enum.TryParse<CommitAction>(action, ignoreCase: true, out var result))
                throw new ArgumentOutOfRangeException(nameof(action));

            switch (result)
            {
                case CommitAction.Create:
                    return new CommitActionCreateHandler();

                case CommitAction.Delete:
                    return new CommitActionDeleteHandler();

                case CommitAction.Update:
                    return new CommitActionUpdateHandler();

                case CommitAction.Move:
                    return new CommitActionMoveHandler();

                case CommitAction.chmod:
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
