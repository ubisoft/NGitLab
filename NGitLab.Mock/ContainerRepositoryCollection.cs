namespace NGitLab.Mock;

public sealed class ContainerRepositoryCollection : Collection<ContainerRepository>
{
    public ContainerRepositoryCollection(GitLabObject parent)
        : base(parent)
    {
    }
}
