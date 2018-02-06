using NGitLab.Models;
using System.Collections.Generic;

namespace NGitLab.Impl
{
    class ContributorClient: IContributorClient
    {
        private readonly API _api;
        private readonly string _contributorPath;

        public ContributorClient(API api, string repoPath)
        {
            _api = api;
            _contributorPath = repoPath + Contributor.Url;
        }
        public IEnumerable<Contributor> All => _api.Get().GetAll<Contributor>(_contributorPath);
    }
}
