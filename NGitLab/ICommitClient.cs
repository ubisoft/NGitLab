using System;
using NGitLab.Models;

namespace NGitLab
{
    public interface ICommitClient
    {
        /// <summary>
        /// Returns the status of a branch.
        /// </summary>
        [Obsolete("This endpoint does not always return data consistent with the pipeline list. " +
                  "Consider using PipelineClient.Search() instead.")]
        JobStatus GetJobStatus(string branchName);

        /// <summary>
        /// Create a commit
        /// </summary>
        Commit Create(CommitCreate commit);

        /// <summary>
        /// Get a specific commit identified by the commit hash or name of a branch or tag.
        /// </summary>
        Commit GetCommit(string @ref);
    }
}
