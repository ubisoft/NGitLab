using System;
using NGitLab.Models;

namespace NGitLab.Impl
{
    /// <summary>
    /// Since GitLab API does not yet return the deleted, added and files count, we need a tool to circumvent that issue
    /// https://gitlab.com/gitlab-org/gitlab/-/issues/233052
    /// This implementation could be also replace by a git log command
    /// </summary>
    public class GitLabChangeDiffCounter : IGitDiffChangeCounter
    {
        public DiffStats Compute(MergeRequestChange mergeRequestChange)
        {
            var totalAdded = 0;
            var totalDeleted = 0;
            var diffStats = new DiffStats();
            foreach (var change in mergeRequestChange.Changes)
            {
                var lines = change.Diff.Split(Environment.NewLine.ToCharArray());
                var added = 0;
                var deleted = 0;
                foreach (var line in lines)
                {
                    if (line.StartsWith("+", StringComparison.InvariantCulture))
                    {
                        diffStats.AddedLines++;
                        added++;
                    }
                    else if (line.StartsWith("-", StringComparison.InvariantCulture))
                    {
                        diffStats.DeletedLines++;
                        deleted++;
                    }
                }

                totalAdded += added;
                totalDeleted += deleted;
            }

            return diffStats;
        }
    }
}
