using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class CommitInfoCollection : Collection<CommitInfo>
{
    public CommitInfoCollection(GitLabObject parent)
        : base(parent)
    {
    }

    public CommitInfo GetOrAdd(string sha)
    {
        var commitInfo = this.FirstOrDefault(commit => string.Equals(commit.Sha, sha, StringComparison.OrdinalIgnoreCase));
        if (commitInfo == null)
        {
            commitInfo = new CommitInfo
            {
                Sha = sha,
            };

            Add(commitInfo);
        }

        return commitInfo;
    }
}
