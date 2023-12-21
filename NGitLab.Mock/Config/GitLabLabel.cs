namespace NGitLab.Mock.Config;

/// <summary>
/// Describe a label in a GitLab group/project
/// </summary>
public class GitLabLabel : GitLabObject
{
    public string Name { get; set; }

    /// <summary>
    /// Color in RGB hex format (example: #5884AD)
    /// </summary>
    public string Color { get; set; }

    public string Description { get; set; }
}

public class GitLabLabelsCollection : GitLabCollection<GitLabLabel>
{
    internal GitLabLabelsCollection(GitLabProject parent)
        : base(parent)
    {
    }

    internal GitLabLabelsCollection(GitLabGroup parent)
        : base(parent)
    {
    }
}
