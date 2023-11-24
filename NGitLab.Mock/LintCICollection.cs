namespace NGitLab.Mock;

public sealed class LintCICollection : Collection<LintCI>
{
    public LintCICollection(GitLabObject container)
        : base(container)
    {
    }
}
