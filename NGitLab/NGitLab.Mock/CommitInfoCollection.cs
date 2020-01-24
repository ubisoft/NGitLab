namespace NGitLab.Mock
{
    public sealed class CommitInfoCollection : Collection<CommitInfo>
    {
        public CommitInfoCollection(GitLabObject parent)
            : base(parent)
        {
        }
    }
}