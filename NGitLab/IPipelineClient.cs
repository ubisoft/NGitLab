using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
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
        Pipeline this[int id] { get; }

        Task<Pipeline> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all jobs in a project
        /// </summary>
        IEnumerable<Job> AllJobs { get; }

        GitLabCollectionResponse<Job> GetAllJobsAsync();

        /// <summary>
        /// Get jobs in a project meeting the scope
        /// </summary>
        /// <param name="scope"></param>
        [System.Obsolete("Use JobClient.GetJobs() instead")]
        IEnumerable<Job> GetJobsInProject(JobScope scope);

        /// <summary>
        /// Returns the jobs of a pipeline.
        /// </summary>
        Job[] GetJobs(int pipelineId);

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
        void Delete(int pipelineId);

        /// <summary>
        /// Get variables for a pipeline.
        /// </summary>
        /// <param name="pipelineId">ID of the pipeline</param>
        IEnumerable<PipelineVariable> GetVariables(int pipelineId);

        GitLabCollectionResponse<PipelineVariable> GetVariablesAsync(int pipelineId);

        /// <summary>
        /// Get test reports for a pipeline.
        /// </summary>
        /// <param name="pipelineId">ID of the pipeline</param>
        TestReport GetTestReports(int pipelineId);
    }
}
