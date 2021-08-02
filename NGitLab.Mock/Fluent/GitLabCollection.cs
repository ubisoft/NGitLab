using System.Threading;

namespace NGitLab.Mock.Fluent
{
    public class GitLabCollection<TItem, TParent> : System.Collections.ObjectModel.Collection<TItem>
        where TItem : GitLabObject<TParent>
    {
        private readonly TParent _parent;
        private int _idIncrement;

        protected internal GitLabCollection(TParent parent)
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

        private void SetItem(TItem item)
        {
            if (item == null)
                return;

            item.Parent = _parent;
            item.Id = Interlocked.Increment(ref _idIncrement);
        }
    }
}
