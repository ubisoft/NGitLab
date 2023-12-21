namespace NGitLab.Models;

/// <summary>
/// Type of references to consider when determining those a commit is pushed to.
/// </summary>
/// <see href="https://docs.gitlab.com/ee/api/commits.html#get-references-a-commit-is-pushed-to"/>
public enum CommitRefType
{
    All,
    Branch,
    Tag,
}
