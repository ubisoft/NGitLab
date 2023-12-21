namespace NGitLab.Mock;

public sealed class CommitStatusCollection : Collection<CommitStatus>
{
    public CommitStatusCollection(GitLabObject parent)
        : base(parent)
    {
    }
}
