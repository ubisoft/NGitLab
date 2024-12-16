using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IPipelineScheduleClient
{
    /// <summary>
    /// Gets all the pipeline schedules of the project
    /// </summary>
    IEnumerable<PipelineScheduleBasic> All { get; }

    /// <summary>
    /// Details of single schedule
    /// </summary>
    /// <param name="id">Schedule Id</param>
    /// <returns></returns>
    PipelineSchedule this[long id] { get; }

    /// <summary>
    /// Gets the details of single schedule
    /// </summary>
    /// <param name="id">Schedule Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PipelineSchedule> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all the pipeline schedules of the project
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    GitLabCollectionResponse<PipelineScheduleBasic> GetAllAsync();

    /// <summary>
    /// Gets all pipelines triggered by a pipeline schedule in a project.
    /// </summary>
    /// <param name="id">Schedule Id</param>
    /// <returns></returns>
    GitLabCollectionResponse<PipelineBasic> GetAllSchedulePipelinesAsync(long id);
}
