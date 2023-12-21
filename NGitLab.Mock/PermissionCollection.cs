namespace NGitLab.Mock;

public sealed class PermissionCollection : Collection<Permission>
{
    public PermissionCollection(GitLabObject container)
        : base(container)
    {
    }
}
