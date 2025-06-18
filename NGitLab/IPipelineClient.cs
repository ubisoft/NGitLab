﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IPipelineClient
{
    /// <summary>
    /// All the pipelines of the project.
    /// </summary>
    IEnumerable<PipelineBasic> All { get; }

    /// <summary>
    /// Returns the detail of a single pipeline.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Pipeline this[long id] { get; }

    Task<Pipeline> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all jobs in a project
    /// </summary>
    IEnumerable<Job> AllJobs { get; }

    GitLabCollectionResponse<Job> GetAllJobsAsync();

    /// <summary>
    /// Returns the jobs of a pipeline.
    /// </summary>
    Job[] GetJobs(long pipelineId);

    /// <summary>
    /// Returns the jobs of a pipeline.
    /// </summary>
    IEnumerable<Job> GetJobs(PipelineJobQuery query);

    /// <summary>
    /// Returns the jobs of a pipeline.
    /// </summary>
    /// <remarks>Unlike <see cref="GetJobs(PipelineJobQuery)"/>, this method returns jobs in the same order as sent by the GitLab API</remarks>
    GitLabCollectionResponse<Job> GetJobsAsync(PipelineJobQuery query);

    /// <summary>
    /// Create a new pipeline.
    /// </summary>
    /// <param name="ref">The branch or tag to run the pipeline on</param>
    Pipeline Create(string @ref);

    /// <summary>
    /// Create a new pipeline with a combination of possible create options
    /// </summary>
    /// <param name="createOptions">Pipeline create options</param>
    Pipeline Create(PipelineCreate createOptions);

    Task<Pipeline> CreateAsync(PipelineCreate createOptions, CancellationToken cancellationToken = default);

    /// <summary>
    /// Trigger a pipeline and send some info with a custom variable
    /// </summary>
    /// <param name="token">Token of the trigger</param>
    /// <param name="ref">The branch or tag to run the pipeline on</param>
    /// <param name="variables">Names and values of the custom variables</param>
    /// <returns></returns>
    Pipeline CreatePipelineWithTrigger(string token, string @ref, Dictionary<string, string> variables);

    /// <summary>
    /// Searches for pipelines from <see cref="PipelineQuery"/>.
    /// </summary>
    /// <param name="query">The query to find pipelines with <see cref="PipelineQuery"/></param>
    /// <returns>Returns a list of pipelines (<see cref="PipelineBasic"/>) using the query</returns>
    IEnumerable<PipelineBasic> Search(PipelineQuery query);

    GitLabCollectionResponse<PipelineBasic> SearchAsync(PipelineQuery query);

    /// <summary>
    /// Delete a pipeline.
    /// </summary>
    /// <param name="pipelineId">ID of the pipeline</param>
    void Delete(long pipelineId);

    /// <summary>
    /// Get variables for a pipeline.
    /// </summary>
    /// <param name="pipelineId">ID of the pipeline</param>
    IEnumerable<PipelineVariable> GetVariables(long pipelineId);

    GitLabCollectionResponse<PipelineVariable> GetVariablesAsync(long pipelineId);

    /// <summary>
    /// Get test reports for a pipeline.
    /// </summary>
    /// <param name="pipelineId">ID of the pipeline</param>
    TestReport GetTestReports(long pipelineId);

    /// <summary>
    /// Get test reports summary for a pipeline.
    /// </summary>
    /// <param name="pipelineId">ID of the pipeline</param>
    TestReportSummary GetTestReportsSummary(long pipelineId);

    /// <summary>
    /// Returns the bridges of a pipeline.
    /// </summary>
    /// <param name="query"></param>
    GitLabCollectionResponse<Bridge> GetBridgesAsync(PipelineBridgeQuery query);

    Task<Pipeline> RetryAsync(long pipelineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the metadata for the specified pipeline
    /// </summary>
    /// <param name="pipelineId">ID of the pipeline</param>
    /// <param name="update">The metadata to update</param>
    /// <param name="cancellationToken">The cancellation otken for the operation</param>
    /// <seealso href="https://docs.gitlab.com/ee/api/pipelines.html#update-pipeline-metadata" />
    Task<Pipeline> UpdateMetadataAsync(long pipelineId, PipelineMetadataUpdate update, CancellationToken cancellationToken = default);
}
