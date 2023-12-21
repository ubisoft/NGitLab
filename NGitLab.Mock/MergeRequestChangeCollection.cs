using System;
using LibGit2Sharp;

namespace NGitLab.Mock;

public class MergeRequestChangeCollection : Collection<Change>
{
    public MergeRequestChangeCollection(GitLabObject parent)
        : base(parent)
    {
    }

    [Obsolete("Use Add(string diff, string oldPath, string newPath, ChangeKind? changeKind) instead")]
    public Change Add(string diff)
    {
        return Add(diff, oldPath: null, newPath: null, changeKind: null);
    }

    public Change Add(string diff, string oldPath, string newPath, ChangeKind? changeKind)
    {
        if (diff is null)
            throw new ArgumentNullException(nameof(diff));

        var change = new Change
        {
            Diff = diff,
            NewPath = newPath,
            OldPath = oldPath,
            DeletedFile = changeKind == ChangeKind.Deleted,
            NewFile = changeKind == ChangeKind.Added,
            RenamedFile = changeKind == ChangeKind.Renamed,
        };

        Add(change);
        return change;
    }
}
