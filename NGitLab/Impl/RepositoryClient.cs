using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class RepositoryClient : IRepositoryClient
    {
        private readonly API _api;
        private readonly string _repoPath;
        private readonly string _projectPath;
        private readonly int _projectId;

        public RepositoryClient(API api, int projectId)
        {
            _api = api;
            _projectId = projectId;
            _projectPath = Project.Url + "/" + projectId;
            _repoPath = _projectPath + "/repository";
        }

        public ITagClient Tags => new TagClient(_api, _repoPath);
        public IReleaseClient Releases => new ReleaseClient(_api, _projectPath);

        public IContributorClient Contributors => new ContributorClient(_api, _repoPath, _projectId);

        public IEnumerable<Tree> Tree => _api.Get().GetAll<Tree>(_repoPath + "/tree");

        public IEnumerable<Tree> GetTree(string path) => GetTree(new RepositoryGetTreeOptions { Path = path });

        public IEnumerable<Tree> GetTree(string path, string @ref, bool recursive) => GetTree(new RepositoryGetTreeOptions { Path = path, Ref = @ref, Recursive = recursive });

        public IEnumerable<Tree> GetTree(RepositoryGetTreeOptions options)
        {
            var args = new List<string>(4);

            var uri = $"{_repoPath}/tree";
            if (!string.IsNullOrEmpty(options.Path))
                args.Add($"path={options.Path}");
            if (!string.IsNullOrEmpty(options.Ref))
                args.Add($"ref={Uri.EscapeDataString(options.Ref)}");
            if (options.Recursive)
                args.Add("recursive=true");
            if (options.PerPage.HasValue)
                args.Add($"per_page={options.PerPage.Value.ToString(CultureInfo.InvariantCulture)}");

            if (args.Count > 0)
                uri += "?" + string.Join("&", args);

            return _api.Get().GetAll<Tree>(uri);
        }

        public void GetRawBlob(string sha, Action<Stream> parser)
        {
            _api.Get().Stream(_repoPath + "/raw_blobs/" + sha, parser);
        }

        public void GetArchive(Action<Stream> parser)
        {
            _api.Get().Stream(_repoPath + "/archive", parser);
        }

        public IEnumerable<Commit> Commits => _api.Get().GetAll<Commit>(_repoPath + $"/commits?per_page={GetCommitsRequest.DefaultPerPage}");

        /// <summary>
        /// Gets all the commits of the specified branch/tag.
        /// </summary>
        public IEnumerable<Commit> GetCommits(string refName, int maxResults)
        {
            return GetCommits(new GetCommitsRequest { MaxResults = maxResults, RefName = refName });
        }

        /// <summary>
        /// Gets all the commits of the specified branch/tag.
        /// </summary>
        public IEnumerable<Commit> GetCommits(GetCommitsRequest request)
        {
            var lst = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.RefName))
            {
                lst.Add($"ref_name={Uri.EscapeDataString(request.RefName)}");
            }

            if (!string.IsNullOrWhiteSpace(request.Path))
            {
                lst.Add($"path={Uri.EscapeDataString(request.Path)}");
            }

            if (request.FirstParent != null)
            {
                lst.Add($"first_parent={Uri.EscapeDataString(request.FirstParent.ToString())}");
            }

            var perPage = request.MaxResults > 0 ? Math.Min(request.MaxResults, request.PerPage) : request.PerPage;
            lst.Add($"per_page={perPage}");

            var path = _repoPath + "/commits" + (lst.Count == 0 ? string.Empty : "?" + string.Join("&", lst));
            var allCommits = _api.Get().GetAll<Commit>(path);
            if (request.MaxResults <= 0)
                return allCommits;

            return allCommits.Take(request.MaxResults);
        }

        public Commit GetCommit(Sha1 sha) => _api.Get().To<Commit>(_repoPath + "/commits/" + sha);

        public IEnumerable<Diff> GetCommitDiff(Sha1 sha) => _api.Get().GetAll<Diff>(_repoPath + "/commits/" + sha + "/diff");

        public IEnumerable<Ref> GetCommitRefs(Sha1 sha, CommitRefType type = CommitRefType.All) =>
            _api.Get().GetAll<Ref>($"{_repoPath}/commits/{sha}/refs?type={type.ToString().ToLowerInvariant()}");

        public IFilesClient Files => new FilesClient(_api, _repoPath);

        public IBranchClient Branches => new BranchClient(_api, _repoPath);

        public IProjectHooksClient ProjectHooks => new ProjectHooksClient(_api, _projectPath);
    }
}
