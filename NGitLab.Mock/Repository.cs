﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using LibGit2Sharp;
using NGitLab.Mock.Clients;

namespace NGitLab.Mock
{
    public sealed class Repository : GitLabObject, IDisposable
    {
        private static int _tempBranchName;

        private readonly object _lock = new();
        private TemporaryDirectory _directory;
        private LibGit2Sharp.Repository _repository;
        private readonly IList<ReleaseTag> _releaseTags = new List<ReleaseTag>();

        public Project Project => (Project)Parent;

        internal Repository(Project project)
        {
            Parent = project;
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

                            // libgit2sharp cannot init with a branch other than master
                            // Use symbolic-ref to keep the code compatible with older version of git
                            using var process = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "git",
                                    Arguments = $"symbolic-ref HEAD \"refs/heads/{Project.DefaultBranch}\"",
                                    RedirectStandardError = true,
                                    UseShellExecute = false,
                                    WorkingDirectory = directory.FullPath,
                                },
                            };

                            process.Start();
                            process.WaitForExit();
                            if (process.ExitCode != 0)
                            {
                                var error = process.StandardError.ReadToEnd();
                                throw new GitLabException($"Cannot update symbolic ref with branch '{Project.DefaultBranch}' in '{directory.FullPath}': {error}");
                            }
                        }
                        else
                        {
                            LibGit2Sharp.Repository.Clone(Project.ForkedFrom.Repository.FullPath, directory.FullPath);
                        }

                        _repository = new LibGit2Sharp.Repository(directory.FullPath);

                        _repository.Config.Set("receive.advertisePushOptions", value: true);
                        _repository.Config.Set("uploadpack.allowFilter", value: true);
                        _repository.Config.Set("receive.denyCurrentBranch", value: "updateInstead"); // Allow git push to existing branches

                        _directory = directory;
                    }
                }
            }

            return _repository;
        }

        public void SetRepoConfig<T>(string name, T value)
        {
            GetGitRepository().Config.Set(name, value);
        }

        public Commit FindMergeBase(Commit first, Commit second)
        {
            var repository = GetGitRepository();
            return repository.ObjectDatabase.FindMergeBase(first, second);
        }

        public bool IsRebaseNeeded(string branch, string ontoRebaseBranch)
        {
            var repository = GetGitRepository();

            var branchCommit = GetCommit(branch);
            var ontoRebaseCommit = GetCommit(ontoRebaseBranch);

            var commonCommit = repository.ObjectDatabase.FindMergeBase(branchCommit, ontoRebaseCommit);

            return commonCommit != ontoRebaseCommit;
        }

        public Branch GetBranch(string branchName)
        {
            if (string.IsNullOrEmpty(branchName))
                return null;
            var repository = GetGitRepository();
            return repository.Branches[branchName];
        }

        public IReadOnlyCollection<Branch> GetAllBranches()
        {
            var repository = GetGitRepository();
            return repository.Branches.ToList();
        }

        public IReadOnlyCollection<Models.ProtectedBranch> GetAllProtectedBranches()
        {
            var repository = GetGitRepository();
            return repository.Branches.Select(b => new Models.ProtectedBranch { Name = b.CanonicalName }).ToList();
        }

        public Commit Commit(User user, string message)
        {
            return Commit(user, message, targetBranch: null, new[] { File.CreateFromText("test.txt", Guid.NewGuid().ToString()) });
        }

        public Commit Commit(User user, string message, IEnumerable<File> files)
        {
            return Commit(user, message, targetBranch: null, files);
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
            var mustCreateBranch = !repo.Commits.Any();
            if (!mustCreateBranch)
            {
                Commands.Checkout(repo, commitCreate.Branch);
            }

            var commit = CreateOnNonBareRepo(commitCreate);
            var branchStillMissing = !repo.Branches.Any(b => string.Equals(b.FriendlyName, commitCreate.Branch, StringComparison.Ordinal));
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
            Commands.Checkout(repository, Project.DefaultBranch);
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

        public IEnumerable<Commit> GetCommits()
        {
            return GetCommits(string.Empty);
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
                return _repository?.Commits.QueryBy(filter).ToList() ?? Enumerable.Empty<Commit>();
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
                return commit;

            return repository.Commits.SingleOrDefault(c => string.Equals(c.Sha, reference, StringComparison.Ordinal));
        }

        public Patch GetBranchFullPatch(string branchName)
        {
            var branch = GetBranch(branchName);
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Reverse,
                IncludeReachableFrom = branchName,
            };
            var sourceCommit = GetGitRepository().Commits.QueryBy(filter).First();
            return GetGitRepository().Diff.Compare<Patch>(sourceCommit.Tree, branch.Tip.Tree);
        }

        public IEnumerable<Commit> GetCommits(GetCommitsRequest request)
        {
            var filter = new CommitFilter
            {
                SortBy = CommitSortStrategies.Topological | CommitSortStrategies.Reverse,
            };

            if (!string.IsNullOrEmpty(request.RefName))
            {
                if (request.RefName.Contains("..", StringComparison.Ordinal))
                {
                    var refs = request.RefName.Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
                    filter.ExcludeReachableFrom = refs[0];
                    filter.IncludeReachableFrom = refs[1];
                }
                else
                {
                    filter.IncludeReachableFrom = request.RefName;
                }
            }

            if (request.FirstParent != null)
            {
                filter.FirstParentOnly = (bool)request.FirstParent;
            }

            try
            {
                return _repository?.Commits.QueryBy(filter).ToList() ?? Enumerable.Empty<Commit>();
            }
            catch (NotFoundException)
            {
                return Enumerable.Empty<Commit>();
            }
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

        internal IEnumerable<Models.Tree> GetTree()
        {
            var repo = GetGitRepository();
            Commands.Checkout(repo, Project.DefaultBranch);

            foreach (var fileSystemEntry in Directory.EnumerateFileSystemEntries(FullPath))
            {
                yield return GetTreeItem(FullPath, fileSystemEntry);
            }
        }

        internal IEnumerable<Models.Tree> GetTree(Models.RepositoryGetTreeOptions repositoryGetTreeOptions)
        {
            var repo = GetGitRepository();
            var commitOrBranch = string.IsNullOrEmpty(repositoryGetTreeOptions.Ref) ? Project.DefaultBranch : repositoryGetTreeOptions.Ref;
            Commands.Checkout(repo, commitOrBranch);

            var searchOption = repositoryGetTreeOptions.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var fullPath = string.IsNullOrEmpty(repositoryGetTreeOptions.Path) ? FullPath : Path.Combine(FullPath, repositoryGetTreeOptions.Path);
            foreach (var fileSystemEntry in Directory.EnumerateFileSystemEntries(fullPath, "*", searchOption))
            {
                yield return GetTreeItem(FullPath, fileSystemEntry);
            }
        }

        internal string FetchBranchFromFork(Project fork, string branch)
        {
            if (fork?.ForkedFrom != Project)
                throw new InvalidOperationException("Cannot fetch MR source branch from unrelated project");

            var repo = GetGitRepository();
            var forkRemoteName = "fork" + fork.Id.ToString(CultureInfo.InvariantCulture);
            var forkRemote =
                repo.Network.Remotes[forkRemoteName] ??
                repo.Network.Remotes.Add(forkRemoteName, fork.Repository.FullPath);

            Commands.Fetch(repo, forkRemote.Name, new[] { branch }, new FetchOptions { }, logMessage: "");

            return $"remotes/{forkRemote.Name}/{branch}";
        }

        private static Models.Tree GetTreeItem(string repositoryPath, string filePath)
        {
            var fileAttribute = System.IO.File.GetAttributes(filePath);
            return new Models.Tree
            {
                Name = Path.GetFileName(filePath),
                Path = GetTreeRelativePath(filePath),
                Type = fileAttribute.HasFlag(FileAttributes.Directory) ? Models.ObjectType.tree : Models.ObjectType.blob,
            };

            string GetTreeRelativePath(string fileFullPath)
            {
                // Directories needs to end with a separator to be considered as one for a Uri
                if (!repositoryPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                    repositoryPath += Path.DirectorySeparatorChar;

                return GetRelativePath(repositoryPath, fileFullPath);
            }
        }

        private static string GetRelativePath(string path, string relativeTo)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(relativeTo))
                throw new ArgumentNullException(nameof(relativeTo));

            if (!Uri.TryCreate(path, UriKind.Absolute, out var pathUri) || !Uri.TryCreate(relativeTo, UriKind.Absolute, out var relativeToUri))
                throw new GitLabException($"Failed to get Uri out of '{path}' or '{relativeTo}'");

            if (!string.Equals(pathUri.Scheme, relativeToUri.Scheme, StringComparison.OrdinalIgnoreCase))
                return relativeTo;

            var relativeUri = pathUri.MakeRelativeUri(relativeToUri);
            return Uri.UnescapeDataString(relativeUri.ToString())!;
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

            var id = Interlocked.Increment(ref _tempBranchName).ToString(CultureInfo.InvariantCulture);
            var tempBranchName = "merge-" + id;
            var tempRemoteName = "origin-" + id;

            var remote = repo.Network.Remotes.Add(tempRemoteName, targetProject.Repository.FullPath);
            var tempBranch = repo.CreateBranch(tempBranchName, branch.Tip);
            repo.Branches.Update(tempBranch, b => b.Remote = tempRemoteName, b => b.UpstreamBranch = tempBranch.CanonicalName);

            // libgit2sharp cannot push to non-bare local repo: repo.Network.Push(remote, branch.CanonicalName, options);
            using var process = Process.Start("git", $"push {tempBranch.CanonicalName}");
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new GitLabException("Cannot push branch");

            repo.Network.Remotes.Remove(tempRemoteName);
            repo.Branches.Remove(tempBranch);

            var merger = new Signature(user.UserName, user.Email, DateTimeOffset.UtcNow);
            Commands.Checkout(targetGitRepository, targetBranch);
            var result = targetGitRepository.Merge(tempBranchName, merger);
            targetGitRepository.Branches.Remove(tempBranchName);

            return result.Commit;
        }

        public bool Rebase(UserRef user, string sourceBranch, string targetBranch)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(sourceBranch))
                throw new ArgumentException("Cannot be null or empty", nameof(sourceBranch));
            if (string.IsNullOrEmpty(targetBranch))
                throw new ArgumentException("Cannot be null or empty", nameof(targetBranch));

            var repo = GetGitRepository();
            var rebase = repo.Rebase;

            var branch = repo.Branches[sourceBranch];
            var upstream = repo.Branches[targetBranch];

            var committer = new Identity(user.UserName, user.Email);
            var options = new RebaseOptions();

            _ = repo.Network.Remotes[targetBranch];

            var rebaseResult = rebase.Start(branch, upstream, onto: null, committer, options);
            if (rebaseResult.Status == RebaseStatus.Conflicts)
            {
                rebase.Abort();
                return false;
            }

            return true;
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

            return result switch
            {
                CommitAction.Create => new CommitActionCreateHandler(),
                CommitAction.Delete => new CommitActionDeleteHandler(),
                CommitAction.Update => new CommitActionUpdateHandler(),
                CommitAction.Move => new CommitActionMoveHandler(),
                _ => throw new NotSupportedException(),
            };
        }

        internal int ComputeDivergence(Commit first, Commit second)
        {
            var repository = GetGitRepository();
            var historyDivergence = repository.ObjectDatabase.CalculateHistoryDivergence(first, second);

            var divergence = 0;
            if (historyDivergence.AheadBy.HasValue)
                divergence += historyDivergence.AheadBy.Value;
            if (historyDivergence.BehindBy.HasValue)
                divergence += historyDivergence.BehindBy.Value;

            return divergence;
        }
    }
}
