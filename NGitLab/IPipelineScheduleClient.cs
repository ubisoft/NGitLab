using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IPipelineScheduleClient
{
    /// <summary>
    /// All the scheduled pipelines of the project
    /// </summary>
    IEnumerable<PipelineScheduleBasic> All { get; }

    /// <summary>
    /// Return the details of single schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    PipelineSchedule this[int id] { get; }

    /// <summary>
    /// Return the details of single schedule
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PipelineSchedule> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// All the scheduled pipelines of the project
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    GitLabCollectionResponse<PipelineScheduleBasic> GetAllAsync(CancellationToken cancellationToken = default);
}
