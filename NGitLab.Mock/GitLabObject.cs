namespace NGitLab.Mock;

public abstract class GitLabObject
{
    public GitLabObject Parent { get; internal set; }

    public GitLabServer Server => this is GitLabServer server ? server : Parent?.Server;
}
