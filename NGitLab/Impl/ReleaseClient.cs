using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

// Handles interacting with Releases.
// Documentation: https://docs.gitlab.com/ee/api/releases/
internal sealed class ReleaseClient : IReleaseClient
{
    private readonly API _api;
    private readonly string _releasesPath;

    public ReleaseClient(API api, ProjectId projectId)
    {
        _api = api;
        var projectPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
        _releasesPath = $"{projectPath}/releases";
    }

    public IEnumerable<ReleaseInfo> All => _api.Get().GetAll<ReleaseInfo>(_releasesPath);

    public ReleaseInfo this[string tagName] => _api.Get().To<ReleaseInfo>($"{_releasesPath}/{Uri.EscapeDataString(tagName)}");

    public ReleaseInfo Create(ReleaseCreate data) => _api.Post().With(data).To<ReleaseInfo>(_releasesPath);

    public Task<ReleaseInfo> CreateAsync(ReleaseCreate data, CancellationToken cancellationToken = default) => _api.Post().With(data).ToAsync<ReleaseInfo>(_releasesPath, cancellationToken);

    public ReleaseInfo Update(ReleaseUpdate data) => _api.Put().With(data).To<ReleaseInfo>($"{_releasesPath}/{Uri.EscapeDataString(data.TagName)}");

    public void Delete(string tagName) => _api.Delete().Execute($"{_releasesPath}/{Uri.EscapeDataString(tagName)}");

    public IReleaseLinkClient ReleaseLinks(string tagName) => new ReleaseLinkClient(_api, _releasesPath, tagName);

    public GitLabCollectionResponse<ReleaseInfo> GetAsync(ReleaseQuery query = null)
    {
        var url = _releasesPath;
        if (query != null)
        {
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);
            url = Utils.AddParameter(url, "include_html_description", query.IncludeHtmlDescription);
            url = Utils.AddParameter(url, "per_page", query.PerPage);
            url = Utils.AddParameter(url, "page", query.Page);
        }

        return _api.Get().GetAllAsync<ReleaseInfo>(url);
    }
}
