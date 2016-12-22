using System.Linq;
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

        public string GetBuildStatus(string branchName)
        {
            var latestCommit = _api.Get().To<Commit>(_repoPath + $"/commits/{branchName}?per_page=1");
            return latestCommit?.Status;
        }
    }

    public static class BuildStatuses
    {
        public const string Running = "running";
        public const string Pending = "pending";
        public const string Failed = "failed";
        public const string Passed = "passed";
    }
}