using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class MilestoneCollection : Collection<Milestone>
{
    public MilestoneCollection(GitLabObject container)
        : base(container)
    {
    }

    public Milestone GetByIid(long iid)
    {
        return this.FirstOrDefault(i => i.Iid == iid);
    }

    public override void Add(Milestone item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = Server.GetNewMilestoneId();
        }

        if (item.Iid == default)
        {
            item.Iid = GetNewIid();
        }
        else if (GetByIid(item.Iid) != null)
        {
            throw new GitLabException("Milestone already exists");
        }

        base.Add(item);
    }

    private long GetNewIid()
    {
        return this.Select(i => i.Iid).DefaultIfEmpty().Max() + 1;
    }
}
