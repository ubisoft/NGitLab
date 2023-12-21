using System;
using NGitLab.Models;

namespace NGitLab.Impl;

/// <summary>
/// Since GitLab API does not yet return the deleted, added and files count, we need a tool to circumvent that issue
/// https://gitlab.com/gitlab-org/gitlab/-/issues/233052
/// This implementation could be also replace by a git log command
/// </summary>
public class GitLabChangeDiffCounter : IGitDiffChangeCounter
{
    public DiffStats Compute(MergeRequestChange mergeRequestChange)
    {
        var diffStats = new DiffStats();
        foreach (var change in mergeRequestChange.Changes)
        {
            var lines = change.Diff.Split(Environment.NewLine.ToCharArray());
            foreach (var line in lines)
            {
                if (line.StartsWith("+", StringComparison.InvariantCulture))
                {
                    diffStats.AddedLines++;
                }
                else if (line.StartsWith("-", StringComparison.InvariantCulture))
                {
                    diffStats.DeletedLines++;
                }
            }
        }

        return diffStats;
    }
}
