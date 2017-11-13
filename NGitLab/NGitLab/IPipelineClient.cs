using System.Collections.Generic;
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

        /// <summary>
        /// Get all jobs in a project
        /// </summary>
        IEnumerable<Job> AllJobs { get; }

        /// <summary>
        /// Get jobs in a project meeting the scope
        /// </summary>
        /// <param name="scope"></param>
        IEnumerable<Job> GetJobsInProject(Job.Scope scope);

        /// <summary>
        /// Returns the jobs of a pipeline.
        /// </summary>
        Job[] GetJobs(int pipelineId);
    }
}