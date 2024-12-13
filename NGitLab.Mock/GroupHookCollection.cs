using System;
using System.Linq;

namespace NGitLab.Mock;

public class GroupHookCollection : Collection<GroupHook>
{
    public GroupHookCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(GroupHook item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = GetNewId();
        }

        base.Add(item);
    }

    private long GetNewId()
    {
        return this.Select(hook => hook.Id).DefaultIfEmpty().Max() + 1;
    }
}
