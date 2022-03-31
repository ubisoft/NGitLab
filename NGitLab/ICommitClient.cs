using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get merge requests related to a commit
        /// </summary>
        GitLabCollectionResponse<MergeRequest> GetRelatedMergeRequestsAsync(RelatedMergeRequestsQuery query);
    }
}
