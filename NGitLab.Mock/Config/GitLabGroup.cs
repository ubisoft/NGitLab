using NGitLab.Models;

namespace NGitLab.Mock.Config;

/// <summary>
/// Describe a GitLab group
/// </summary>
public class GitLabGroup : GitLabObject<GitLabConfig>
{
    public GitLabGroup()
    {
        Labels = new GitLabLabelsCollection(this);
        Permissions = new GitLabPermissionsCollection(this);
        Milestones = new GitLabMilestonesCollection(this);
    }

    /// <summary>
    /// Name (required)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Path/slug. Defaults to <see cref="Slug.Create(Name)"/>.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Parent namespace
    /// </summary>
    public string Namespace { get; set; }

    public string Description { get; set; }

    public VisibilityLevel? Visibility { get; set; }

    public GitLabLabelsCollection Labels { get; }

    public GitLabPermissionsCollection Permissions { get; }

    public GitLabMilestonesCollection Milestones { get; }
}

public class GitLabGroupsCollection : GitLabCollection<GitLabGroup, GitLabConfig>
{
    internal GitLabGroupsCollection(GitLabConfig parent)
        : base(parent)
    {
    }
}
