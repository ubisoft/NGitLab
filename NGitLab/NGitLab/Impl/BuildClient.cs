using System;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class BuildClient : IBuildClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public BuildClient(API api, int projectId)
        {
            _api = api;

            var projectPath = Project.Url + "/" + projectId;
            _repoPath = projectPath + "/repository";
        }

        public BuildStatus GetBuildStatus(string branchName)
        {
            var latestCommit = _api.Get().To<Commit>(_repoPath + $"/commits/{branchName}?per_page=1");
            if (latestCommit == null)
            {
                return BuildStatus.Unknown;
            }
            
            BuildStatus result;
            if (!Enum.TryParse(latestCommit.Status, ignoreCase: true, result: out result))
            {
                throw new NotImplementedException($"Status {latestCommit.Status} is unrecognised");
            }

            return result;
        }
    }
}