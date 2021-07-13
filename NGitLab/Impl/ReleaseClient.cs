using System.Collections.Generic;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl
{
    // Handles interacting with Releases.
    // Documentation: https://docs.gitlab.com/ee/api/releases/
    internal class ReleaseClient : IReleaseClient
    {
        private readonly API _api;
        private readonly int _projectId;
        private readonly string _releasesPath;

        public ReleaseClient(API api, int projectId)
        {
            _api = api;
            _projectId = projectId;
            var projectPath = Project.Url + "/" + projectId;
            _releasesPath = projectPath + "/releases";
        }

        public IEnumerable<ReleaseInfo> All  => _api.Get().GetAll<ReleaseInfo>(_releasesPath);

        public ReleaseInfo this[string tagName] => _api.Get().To<ReleaseInfo>($"{_releasesPath}/{WebUtility.UrlEncode(tagName)}");

        public ReleaseInfo Create(ReleaseCreate data) => _api.Post().With(data).To<ReleaseInfo>(_releasesPath);

        public ReleaseInfo Update(ReleaseUpdate data) => _api.Put().With(data).To<ReleaseInfo>($"{_releasesPath}/{WebUtility.UrlEncode(data.TagName)}");

        public void Delete(string tagName) => _api.Delete().Execute($"{_releasesPath}/{WebUtility.UrlEncode(tagName)}");

        public IReleaseLinkClient ReleaseLinks(string tagName) => new ReleaseLinkClient(_api, _releasesPath, tagName);
    }
}
