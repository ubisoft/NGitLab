using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class RepositoryClient : IRepositoryClient
    {
        private readonly API _api;
        private readonly string _repoPath;
        private readonly string _projectPath;

        public RepositoryClient(API api, int projectId)
        {
            _api = api;

            _projectPath = Project.Url + "/" + projectId;
            _repoPath = _projectPath + "/repository";
        }

        public IEnumerable<Tag> Tags => _api.Get().GetAll<Tag>(_repoPath + "/tags");

        public IEnumerable<TreeOrBlob> Tree => _api.Get().GetAll<TreeOrBlob>(_repoPath + "/tree");

        public void GetRawBlob(string sha, Action<Stream> parser)
        {
            _api.Get().Stream(_repoPath + "/raw_blobs/" + sha, parser);
        }

        public void GetArchive(Action<Stream> parser)
        {
            _api.Get().Stream(_repoPath + "/archive", parser);
        }

        public IEnumerable<Commit> Commits => _api.Get().GetAll<Commit>(_repoPath + "/commits");

        /// <summary>
        /// Gets all the commits of the specified branch/tag.
        /// </summary>
        public IEnumerable<Commit> GetCommits(string refName, int maxResults) => _api.Get().GetAll<Commit>(_repoPath + $"/commits?ref_name={refName}&{GetPagination(maxResults)}");

        private string GetPagination(int maxResults)
        {
            return maxResults == 0 ? "" : $"per_page={maxResults}";
        }

        public SingleCommit GetCommit(Sha1 sha) => _api.Get().To<SingleCommit>(_repoPath + "/commits/" + sha);

        public IEnumerable<Diff> GetCommitDiff(Sha1 sha) => _api.Get().GetAll<Diff>(_repoPath + "/commits/" + sha + "/diff");

        public IFilesClient Files => new FileClient(_api, _repoPath);

        public IBranchClient Branches => new BranchClient(_api, _repoPath);

        public IProjectHooksClient ProjectHooks => new ProjectHooksClient(_api, _projectPath);
    }
}