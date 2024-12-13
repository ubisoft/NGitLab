using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class EnvironmentClient : IEnvironmentClient
{
    private readonly API _api;
    private readonly string _environmentsPath;

    public EnvironmentClient(API api, ProjectId projectId)
    {
        _api = api;
        _environmentsPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}/environments";
    }

    public IEnumerable<EnvironmentInfo> All => _api.Get().GetAll<EnvironmentInfo>(_environmentsPath);

    public EnvironmentInfo Create(string name, string externalUrl)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        var url = Utils.AddParameter(_environmentsPath, "name", name);

        if (!string.IsNullOrEmpty(externalUrl))
        {
            url = Utils.AddParameter(url, "external_url", externalUrl);
        }

        return _api.Post().To<EnvironmentInfo>(url);
    }

    public EnvironmentInfo Edit(long environmentId, string externalUrl) => Edit(environmentId, null, externalUrl);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public EnvironmentInfo Edit(long environmentId, string name, string externalUrl)
    {
        var url = $"{_environmentsPath}/{environmentId.ToStringInvariant()}";

        if (!string.IsNullOrEmpty(name))
        {
            url = Utils.AddParameter(url, "name", name);
        }

        if (!string.IsNullOrEmpty(externalUrl))
        {
            url = Utils.AddParameter(url, "external_url", externalUrl);
        }

        return _api.Put().To<EnvironmentInfo>(url);
    }

    public void Delete(long environmentId) => _api.Delete().Execute($"{_environmentsPath}/{environmentId.ToStringInvariant()}");

    public EnvironmentInfo Stop(long environmentId)
    {
        return _api.Post().To<EnvironmentInfo>($"{_environmentsPath}/{environmentId.ToStringInvariant()}/stop");
    }

    public GitLabCollectionResponse<EnvironmentInfo> GetEnvironmentsAsync(EnvironmentQuery query)
    {
        var url = _environmentsPath;

        url = Utils.AddParameter(url, "name", value: query.Name);
        url = Utils.AddParameter(url, "search", value: query.Search);
        url = Utils.AddParameter(url, "states", value: query.State);

        return _api.Get().GetAllAsync<EnvironmentInfo>(url);
    }

    public EnvironmentInfo GetById(long environmentId)
    {
        return _api.Get().To<EnvironmentInfo>($"{_environmentsPath}/{environmentId.ToStringInvariant()}");
    }

    public Task<EnvironmentInfo> GetByIdAsync(long environmentId, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<EnvironmentInfo>($"{_environmentsPath}/{environmentId.ToStringInvariant()}", cancellationToken);
    }
}
