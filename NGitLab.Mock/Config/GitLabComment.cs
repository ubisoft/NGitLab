using System;

namespace NGitLab.Mock.Config;

public class GitLabComment : GitLabObject
{
    /// <summary>
    /// Author username (required if default user not defined)
    /// </summary>
    public string Author { get; set; }

    public string Message { get; set; }

    /// <summary>
    /// Indicates if comment is from GitLab system
    /// </summary>
    public bool System { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Comment thread (all comments with same thread are grouped)
    /// </summary>
    public string Thread { get; set; }

    /// <summary>
    /// Indicates if comment is resolvable (only for merge request comments)
    /// </summary>
    public bool Resolvable { get; set; }

    /// <summary>
    /// Indicates if comment is resolved (only for merge request comments and resolvable)
    /// </summary>
    public bool Resolved { get; set; }
}

public class GitLabCommentsCollection : GitLabCollection<GitLabComment>
{
    internal GitLabCommentsCollection(GitLabIssue parent)
        : base(parent)
    {
    }

    internal GitLabCommentsCollection(GitLabMergeRequest parent)
        : base(parent)
    {
    }
}
