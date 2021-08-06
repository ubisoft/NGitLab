using System.Threading;

namespace NGitLab.Mock.Config
{
    public class GitLabCollection<TItem> : System.Collections.ObjectModel.Collection<TItem>
        where TItem : GitLabObject
    {
        internal readonly object _parent;
        private int _idIncrement;

        protected internal GitLabCollection(object parent)
        {
            _parent = parent;
        }

        protected override void InsertItem(int index, TItem item)
        {
            SetItem(item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, TItem item)
        {
            SetItem(item);
            base.SetItem(index, item);
        }

        internal virtual void SetItem(TItem item)
        {
            if (item == null)
                return;

            item.Parent = _parent;

            if (item.Id == default)
                item.Id = Interlocked.Increment(ref _idIncrement);
        }
    }

    public class GitLabCollection<TItem, TParent> : GitLabCollection<TItem>
        where TItem : GitLabObject<TParent>
    {
        protected internal GitLabCollection(TParent parent)
            : base(parent)
        {
        }
    }
}
