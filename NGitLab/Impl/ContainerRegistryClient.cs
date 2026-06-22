using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class ContainerRegistryClient : IContainerRegistryClient
{
    private readonly API _api;
    private readonly string _projectId;

    public ContainerRegistryClient(API api, ProjectId projectId)
    {
        _api = api;
        _projectId = projectId.ValueAsUriParameter();
    }

    public GitLabCollectionResponse<ContainerRepository> GetRepositoriesAsync(CancellationToken cancellationToken = default)
        => _api.Get().GetAllAsync<ContainerRepository>($"/projects/{_projectId}/registry/repositories");

    public GitLabCollectionResponse<ContainerRegistryTag> GetTagsAsync(long repositoryId, CancellationToken cancellationToken = default)
        => _api.Get().GetAllAsync<ContainerRegistryTag>($"/projects/{_projectId}/registry/repositories/{repositoryId.ToStringInvariant()}/tags");

    public Task DeleteTagAsync(long repositoryId, string tagName, CancellationToken cancellationToken = default)
        => _api.Delete().ExecuteAsync($"/projects/{_projectId}/registry/repositories/{repositoryId.ToStringInvariant()}/tags/{tagName}", cancellationToken);

    public Task DeleteRepositoryAsync(long repositoryId, CancellationToken cancellationToken = default)
        => _api.Delete().ExecuteAsync($"/projects/{_projectId}/registry/repositories/{repositoryId.ToStringInvariant()}", cancellationToken);
}
