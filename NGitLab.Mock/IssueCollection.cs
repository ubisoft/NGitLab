using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class IssueCollection : Collection<Issue>
{
    public IssueCollection(GitLabObject container)
        : base(container)
    {
    }

    public Issue GetByIid(long issueId)
    {
        return this.FirstOrDefault(i => i.Iid == issueId);
    }

    public override void Add(Issue item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == default)
        {
            item.Id = Server.GetNewIssueId();
        }

        if (item.Iid == default)
        {
            item.Iid = GetNewIid();
        }
        else if (GetByIid(item.Iid) != null)
        {
            throw new GitLabException("Issue already exists");
        }

        base.Add(item);
    }

    private long GetNewIid()
    {
        return this.Select(i => i.Iid).DefaultIfEmpty().Max() + 1;
    }
}
