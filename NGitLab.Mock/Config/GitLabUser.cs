namespace NGitLab.Mock.Config;

/// <summary>
/// Describe a GitLab user
/// </summary>
public class GitLabUser : GitLabObject<GitLabConfig>
{
    /// <summary>
    /// Username (required)
    /// </summary>
    public string Username { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string AvatarUrl { get; set; }

    public bool IsAdmin { get; set; }
}

public class GitLabUsersCollection : GitLabCollection<GitLabUser, GitLabConfig>
{
    internal GitLabUsersCollection(GitLabConfig parent)
        : base(parent)
    {
    }
}
