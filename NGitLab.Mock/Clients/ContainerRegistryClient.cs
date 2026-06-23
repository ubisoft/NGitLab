using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;
using ModelContainerRegistryTag = NGitLab.Models.ContainerRegistryTag;
using ModelContainerRepository = NGitLab.Models.ContainerRepository;

namespace NGitLab.Mock.Clients;

internal sealed class ContainerRegistryClient(ClientContext context, ProjectId projectId) : ClientBase(context), IContainerRegistryClient
{
    public GitLabCollectionResponse<ModelContainerRepository> GetRepositoriesAsync(CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var result = project.ContainerRepositories
                .Select(r => r.ToClientContainerRepository())
                .ToList();
            return GitLabCollectionResponse.Create(result);
        }
    }

    public GitLabCollectionResponse<ModelContainerRegistryTag> GetTagsAsync(long repositoryId, CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var repo = project.ContainerRepositories.FirstOrDefault(r => r.Id == repositoryId)
                ?? throw GitLabException.NotFound();
            var result = repo.Tags
                .Select(t => t.ToClientContainerRegistryTag())
                .ToList();
            return GitLabCollectionResponse.Create(result);
        }
    }

    public Task DeleteTagAsync(long repositoryId, string tagName, CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var repo = project.ContainerRepositories.FirstOrDefault(r => r.Id == repositoryId)
                ?? throw GitLabException.NotFound();
            repo.Tags.RemoveAll(t => string.Equals(t.Name, tagName, System.StringComparison.Ordinal));
            return Task.CompletedTask;
        }
    }

    public Task DeleteRepositoryAsync(long repositoryId, CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var repo = project.ContainerRepositories.FirstOrDefault(r => r.Id == repositoryId)
                ?? throw GitLabException.NotFound();
            project.ContainerRepositories.Remove(repo);
            return Task.CompletedTask;
        }
    }
}
