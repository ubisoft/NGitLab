using System;

namespace NGitLab.Mock.Config;

/// <summary>
/// Describes a release in a GitLab project
/// </summary>
public class GitLabReleaseInfo : GitLabObject<GitLabProject>
{
    public GitLabReleaseInfo()
    {
    }

    public string Author { get; set; }

    public string TagName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ReleasedAt { get; set; }
}

public class GitLabReleaseInfoCollection : GitLabCollection<GitLabReleaseInfo, GitLabProject>
{
    internal GitLabReleaseInfoCollection(GitLabProject parent)
        : base(parent)
    {
    }
}
