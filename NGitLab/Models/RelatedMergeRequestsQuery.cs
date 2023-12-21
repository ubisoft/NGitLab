namespace NGitLab.Models;

/// <summary>
/// Get merge requests related to a commit
/// </summary>
public sealed class RelatedMergeRequestsQuery
{
    /// <summary>
    /// The commit sha for which you want to get the related MRs
    /// </summary>
    public Sha1 Sha { get; set; }
}
