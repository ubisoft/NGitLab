using System.Linq;

namespace NGitLab.Mock.Config;

public abstract class GitLabCollection<TItem> : System.Collections.ObjectModel.Collection<TItem>
    where TItem : GitLabObject
{
    internal readonly object _parent;

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

        item.ParentObject = _parent;

        if (item.Id == default)
            item.Id = Items.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
    }
}

public abstract class GitLabCollection<TItem, TParent> : GitLabCollection<TItem>
    where TItem : GitLabObject<TParent>
{
    protected internal GitLabCollection(TParent parent)
        : base(parent)
    {
    }
}
