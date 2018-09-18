using System;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class CommitClient : ICommitClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public CommitClient(API api, int projectId)
        {
            _api = api;

            var projectPath = Project.Url + "/" + projectId;
            _repoPath = projectPath + "/repository";
        }

        public Commit GetCommit(string @ref)
        {
            return _api.Get().To<Commit>(_repoPath + $"/commits/{@ref}");
        }

        public JobStatus GetJobStatus(string branchName)
        {
            var latestCommit = _api.Get().To<Commit>(_repoPath + $"/commits/{branchName}?per_page=1");
            if (latestCommit == null)
            {
                return JobStatus.Unknown;
            }

            JobStatus result;
            if (!Enum.TryParse(latestCommit.Status, ignoreCase: true, result: out result))
            {
                throw new NotSupportedException($"Status {latestCommit.Status} is unrecognised");
            }

            return result;
        }

        public Commit Create(CommitCreate commit) => _api.Post().With(commit).To<Commit>(_repoPath + "/commits");
    }
}
