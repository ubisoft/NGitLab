using System.Collections.Generic;
using System.Globalization;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl;

// Handles interacting with Release links.
// Documentation: https://docs.gitlab.com/ee/api/releases/links
internal sealed class ReleaseLinkClient : IReleaseLinkClient
{
    private readonly API _api;
    private readonly string _linksPath;

    public ReleaseLinkClient(API api, string releasesPath, string tagName)
    {
        _api = api;
        _linksPath = $"{releasesPath}/{WebUtility.UrlEncode(tagName)}/assets/links";
    }

    public IEnumerable<ReleaseLink> All => _api.Get().GetAll<ReleaseLink>(_linksPath);

    public ReleaseLink this[long linkId] => _api.Get().To<ReleaseLink>($"{_linksPath}/{linkId.ToString(CultureInfo.InvariantCulture)}");

    public ReleaseLink Create(ReleaseLinkCreate data) => _api.Post().With(data).To<ReleaseLink>(_linksPath);

    public ReleaseLink Update(long id, ReleaseLinkUpdate data) => _api.Put().With(data).To<ReleaseLink>($"{_linksPath}/{id.ToString(CultureInfo.InvariantCulture)}");

    public void Delete(long id) => _api.Delete().Execute($"{_linksPath}/{id.ToString(CultureInfo.InvariantCulture)}");
}
