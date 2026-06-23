using System.Collections.Generic;

namespace NGitLab.Mock.Config;

/// <summary>
/// Describe a container repository in a GitLab project
/// </summary>
public class GitLabContainerRepository : GitLabObject
{
    public string Name { get; set; }

    public List<string> Tags { get; } = [];
}

public class GitLabContainerRepositoriesCollection : GitLabCollection<GitLabContainerRepository>
{
    internal GitLabContainerRepositoriesCollection(GitLabProject parent)
        : base(parent)
    {
    }
}
