using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IPipelineClient
    {
        /// <summary>
        /// All the pipelines of the project.
        /// </summary>
        IEnumerable<Pipeline> All { get; }
        
        /// <summary>
        /// Returns the detail of a single pipeline.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Pipeline this[int id] { get; }

        /// <summary>
        /// Returns the jobs of a pipeline.
        /// </summary>
        Job[] GetJobs(int pipelineId);
    }
}