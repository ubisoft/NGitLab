using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IProjectJobTokenScopeClient
{
    /// <summary>
    /// Gets the Project Job Token Scope.
    /// </summary>
    /// <returns>Job Token Scope</returns>
    Task<JobTokenScope> GetProjectJobTokenScopeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the Project Job Token Scope.
    /// </summary>
    Task UpdateProjectJobTokenScopeAsync(JobTokenScope scope, CancellationToken cancellationToken = default);
}
