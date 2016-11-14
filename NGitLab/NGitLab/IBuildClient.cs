namespace NGitLab
{
    public interface IBuildClient
    {
        /// <summary>
        /// Returns the status of a branch.
        /// </summary>
        string GetBuildStatus(string branchName);
    }
}