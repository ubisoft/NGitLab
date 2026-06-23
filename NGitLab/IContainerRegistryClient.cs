using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IContainerRegistryClient
{
    /// <summary>
    /// Returns all container repositories for the project.
    /// </summary>
    GitLabCollectionResponse<ContainerRepository> GetRepositoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all tags for the given repository.
    /// </summary>
    GitLabCollectionResponse<ContainerRegistryTag> GetTagsAsync(long repositoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a single tag from a repository.
    /// </summary>
    Task DeleteTagAsync(long repositoryId, string tagName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a repository and all its tags.
    /// </summary>
    Task DeleteRepositoryAsync(long repositoryId, CancellationToken cancellationToken = default);
}
