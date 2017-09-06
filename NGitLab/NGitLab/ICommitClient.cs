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
    }
}