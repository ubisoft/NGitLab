using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

/// <summary>
/// Uses: https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/runners.md
/// </summary>
public interface IRunnerClient
{
    /// <summary>
    /// Get a list of runners accessible by the authenticated user.
    /// </summary>
    IEnumerable<Runner> Accessible { get; }

    /// <summary>
    /// Get a list of all GitLab runners (admin only).
    /// </summary>
    IEnumerable<Runner> All { get; }

    /// <summary>
    /// Get details of a runner
    /// </summary>
    Runner this[long id] { get; }

    /// <summary>
    /// Deletes the specified runner.
    /// </summary>
    void Delete(Runner runner);

    /// <summary>
    /// Deletes the specified runner.
    /// </summary>
    void Delete(long runnerId);

    /// <summary>
    /// Updates the tags, description, isActive
    /// </summary>
    /// <returns>The updated runner</returns>
    Runner Update(long runnerId, RunnerUpdate runnerUpdate);

    /// <summary>
    /// get the list of runners associated with the group
    /// </summary>
    /// <returns>the list of runners associated with the group</returns>
    IEnumerable<Runner> OfGroup(long groupId);

    GitLabCollectionResponse<Runner> OfGroupAsync(long groupId);

    /// <summary>
    /// get the list of runners associated with the project
    /// </summary>
    /// <returns>the list of runners associated with the project</returns>
    IEnumerable<Runner> OfProject(long projectId);

    GitLabCollectionResponse<Runner> OfProjectAsync(long projectId);

    /// <summary>
    /// List all jobs of the given runner that meet the specified status
    /// </summary>
    IEnumerable<Job> GetJobs(long runnerId, JobStatus? status = null);

    /// <summary>
    /// List all runners (specific and shared) available in the project. Shared runners are listed if at least one shared runner is defined and shared runners usage is enabled in the project's settings.
    /// </summary>
    /// <param name="projectId"></param>
    IEnumerable<Runner> GetAvailableRunners(long projectId);

    /// <summary>
    /// List all runners in the GitLab instance that meet the specified scope
    /// Admin privileges required
    /// </summary>
    /// <param name="scope"></param>
    IEnumerable<Runner> GetAllRunnersWithScope(RunnerScope scope);

    /// <summary>
    /// Enable an available specific runner in the project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="runnerId"></param>
    Runner EnableRunner(long projectId, RunnerId runnerId);

    Task<Runner> EnableRunnerAsync(long projectId, RunnerId runnerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disable a specific runner from the project. It works only if the project isn't the only project associated with the specified runner. If so, an error is returned. Use the Remove a runner call instead.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="runnerId"></param>
    void DisableRunner(long projectId, RunnerId runnerId);

    /// <summary>
    /// Register a new runner for the instance.
    /// <see href="https://docs.gitlab.com/ee/api/runners.html#register-a-new-runner" />
    /// </summary>
    Runner Register(RunnerRegister request);
}
