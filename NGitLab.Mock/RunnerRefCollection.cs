namespace NGitLab.Mock;

public class RunnerRefCollection : Collection<RunnerRef>
{
    public RunnerRefCollection(GitLabObject parent)
        : base(parent)
    {
    }
}
