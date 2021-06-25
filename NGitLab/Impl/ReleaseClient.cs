using System.Collections.Generic;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class ReleaseClient : IReleaseClient
    {
        private readonly API _api;
        private readonly string _releasePath;

        public ReleaseClient(API api, string repoPath)
        {
            _api = api;
            _releasePath = repoPath + "/releases";
        }

        public ReleaseInfo Create(ReleaseCreate release)
        {
            return _api.Post().With(release).To<ReleaseInfo>(_releasePath);
        }

        public ReleaseInfo Update(ReleaseUpdate release)
        {
            return _api.Put().With(release).To<ReleaseInfo>(_releasePath);
        }

        public void Delete(string name)
        {
            _api.Delete().Stream($"{_releasePath}/{WebUtility.UrlEncode(name)}", _ => { });
        }

        public IEnumerable<ReleaseInfo> All => _api.Get().GetAll<ReleaseInfo>(_releasePath + "?per_page=50");
    }
}
