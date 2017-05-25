namespace NGitLab
{
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
        /// <summary>
        /// Manual status means that the job is pending until a user requests it to start.
        /// </summary>
        Manual,
    }
}