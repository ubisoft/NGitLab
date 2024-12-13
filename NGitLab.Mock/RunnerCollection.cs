using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class RunnerCollection : Collection<Runner>
{
    public RunnerCollection(GitLabObject parent)
        : base(parent)
    {
    }

    public override void Add(Runner item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (Server is null)
            throw new ObjectNotAttachedException();

        if (item.Id == default)
        {
            item.Id = Server.GetNewRunnerId();
        }

        item.Token ??= Server.GetNewRunnerToken();

        base.Add(item);
    }

    public bool Remove(long id)
    {
        var r = this.SingleOrDefault(r => r.Id == id);
        return Remove(r);
    }
}
