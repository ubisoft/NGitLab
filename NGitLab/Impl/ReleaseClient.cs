using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    // Handles interacting with Releases.
    // Documentation: https://docs.gitlab.com/ee/api/releases/
    internal sealed class ReleaseClient : IReleaseClient
    {
        private readonly API _api;
        private readonly string _releasesPath;

        public ReleaseClient(API api, int projectId)
        {
            _api = api;
            var projectPath = Project.Url + "/" + projectId.ToStringInvariant();
            _releasesPath = projectPath + "/releases";
        }

        public IEnumerable<ReleaseInfo> All => _api.Get().GetAll<ReleaseInfo>(_releasesPath);

        public ReleaseInfo this[string tagName] => _api.Get().To<ReleaseInfo>($"{_releasesPath}/{Uri.EscapeDataString(tagName)}");

        public ReleaseInfo Create(ReleaseCreate data) => _api.Post().With(data).To<ReleaseInfo>(_releasesPath);

        public Task<ReleaseInfo> CreateAsync(ReleaseCreate data, CancellationToken cancellationToken = default) => _api.Post().With(data).ToAsync<ReleaseInfo>(_releasesPath, cancellationToken);

        public ReleaseInfo Update(ReleaseUpdate data) => _api.Put().With(data).To<ReleaseInfo>($"{_releasesPath}/{Uri.EscapeDataString(data.TagName)}");

        public void Delete(string tagName) => _api.Delete().Execute($"{_releasesPath}/{Uri.EscapeDataString(tagName)}");

        public IReleaseLinkClient ReleaseLinks(string tagName) => new ReleaseLinkClient(_api, _releasesPath, tagName);
    }
}
