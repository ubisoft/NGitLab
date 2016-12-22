namespace NGitLab
{
    public interface IBuildClient
    {
        /// <summary>
        /// Returns the status of a branch.
        /// </summary>
        BuildStatus GetBuildStatus(string branchName);
    }

    public enum BuildStatus
    {
        Unknown,
        Running,
        Pending,
        Failed,
        Success,
        Created,
        Canceled,
        Skipped,
    }
}