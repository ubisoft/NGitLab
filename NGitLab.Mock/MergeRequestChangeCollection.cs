using System;
using LibGit2Sharp;

namespace NGitLab.Mock
{
    public class MergeRequestChangeCollection : Collection<Change>
    {
        public MergeRequestChangeCollection(GitLabObject parent)
            : base(parent)
        {
        }

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
                DeletedFile = changeKind.HasValue && changeKind.Value == ChangeKind.Deleted,
                NewFile = changeKind.HasValue && changeKind.Value == ChangeKind.Added,
                RenamedFile = changeKind.HasValue && changeKind.Value == ChangeKind.Renamed,
            };

            Add(change);
            return change;
        }
    }
}
