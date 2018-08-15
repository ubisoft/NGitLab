using NGitLab.Models;
using System.Collections.Generic;

namespace NGitLab.Impl
{
    internal class ContributorClient : IContributorClient
    {
        private readonly API _api;
        private readonly string _contributorPath;
        private readonly int _projectId;

        public ContributorClient(API api, string repoPath, int projectId)
        {
            _api = api;
            _contributorPath = repoPath + Contributor.Url;
            _projectId = projectId;
        }

        /// <remarks>
        /// HACK: We force the order_by and sort due to a pagination bug from GitLab
        /// </remarks>
        public IEnumerable<Contributor> All => _api.Get().GetAll<Contributor>(_contributorPath + $"?id={_projectId}&order_by=commits&sort=desc");
    }
}
