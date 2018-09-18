using NGitLab.Models;

namespace NGitLab
{
    public interface ICommitClient
    {
        /// <summary>
        /// Returns the status of a branch.
        /// </summary>
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
