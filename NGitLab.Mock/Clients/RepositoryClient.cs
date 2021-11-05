using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class RepositoryClient : ClientBase, IRepositoryClient
    {
        private readonly int _projectId;

        public RepositoryClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public ITagClient Tags => new TagClient(Context, _projectId);

        public IFilesClient Files => new FileClient(Context, _projectId);

        public IBranchClient Branches => new BranchClient(Context, _projectId);

        public IProjectHooksClient ProjectHooks => new ProjectHooksClient(Context, _projectId);

        public IContributorClient Contributors => new ContributorClient(Context, _projectId);

        public IEnumerable<Tree> Tree
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId, ProjectPermission.View);
                    return project.Repository.GetTree();
                }
            }
        }

        public IEnumerable<Commit> Commits
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId, ProjectPermission.View);
                    return project.Repository.GetCommits().Select(commit => ConvertToNGitLabCommit(commit, project));
                }
            }
        }

        public IEnumerable<Tree> GetTree(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tree> GetTree(string path, string @ref, bool recursive)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tree> GetTree(RepositoryGetTreeOptions options)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.Repository.GetTree(options);
            }
        }

        public void GetRawBlob(string sha, Action<Stream> parser)
        {
            throw new NotImplementedException();
        }

        public void GetArchive(Action<Stream> parser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Commit> GetCommits(string refName, int maxResults = 0)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.Repository.GetCommits(refName).Select(commit => ConvertToNGitLabCommit(commit, project));
            }
        }

        public IEnumerable<Commit> GetCommits(GetCommitsRequest request)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.Repository.GetCommits(request).Select(commit => ConvertToNGitLabCommit(commit, project));
            }
        }

        public Commit GetCommit(Sha1 sha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Diff> GetCommitDiff(Sha1 sha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ref> GetCommitRefs(Sha1 sha, CommitRefType type = CommitRefType.All)
        {
            throw new NotImplementedException();
        }

        private Commit ConvertToNGitLabCommit(LibGit2Sharp.Commit commit, Project project)
        {
            return commit.ToCommitClient(project.CommitInfos.SingleOrDefault(c => string.Equals(c.Sha, commit.Sha, StringComparison.Ordinal)));
        }
    }
}
