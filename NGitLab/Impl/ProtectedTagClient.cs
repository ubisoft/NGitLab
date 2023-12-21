using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

public class ProtectedTagClient : IProtectedTagClient
{
    private readonly API _api;
    private readonly string _protectedTagsPath;

    public ProtectedTagClient(API api, ProjectId projectId)
    {
        _api = api;
        _protectedTagsPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}/protected_tags";
    }

    public ProtectedTag ProtectTag(TagProtect protect)
    {
        return _api.Post().With(protect).To<ProtectedTag>(_protectedTagsPath);
    }

    public async Task<ProtectedTag> ProtectTagAsync(TagProtect protect, CancellationToken cancellationToken = default)
    {
        return await _api.Post().With(protect).ToAsync<ProtectedTag>(_protectedTagsPath, cancellationToken).ConfigureAwait(false);
    }

    public void UnprotectTag(string name)
    {
        _api.Delete().Execute($"{_protectedTagsPath}/{Uri.EscapeDataString(name)}");
    }

    public async Task UnprotectTagAsync(string name, CancellationToken cancellationToken = default)
    {
        await _api.Delete().ExecuteAsync($"{_protectedTagsPath}/{Uri.EscapeDataString(name)}", cancellationToken).ConfigureAwait(false);
    }

    public ProtectedTag GetProtectedTag(string name)
    {
        return _api.Get().To<ProtectedTag>($"{_protectedTagsPath}/{Uri.EscapeDataString(name)}");
    }

    public async Task<ProtectedTag> GetProtectedTagAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _api.Get().ToAsync<ProtectedTag>($"{_protectedTagsPath}/{Uri.EscapeDataString(name)}", cancellationToken).ConfigureAwait(false);
    }

    public IEnumerable<ProtectedTag> GetProtectedTags()
    {
        return _api.Get().GetAll<ProtectedTag>(_protectedTagsPath);
    }

    public GitLabCollectionResponse<ProtectedTag> GetProtectedTagsAsync()
    {
        return _api.Get().GetAllAsync<ProtectedTag>(_protectedTagsPath);
    }
}
