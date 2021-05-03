using System;

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
            if (diff is null)
                throw new ArgumentNullException(nameof(diff));

            var change = new Change() { Diff = diff };
            Add(change);
            return change;
        }
    }
}
