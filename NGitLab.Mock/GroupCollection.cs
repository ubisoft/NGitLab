using System;

namespace NGitLab.Mock;

public sealed class GroupCollection : Collection<Group>
{
    public GroupCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(Group group)
    {
        if (group is null)
            throw new ArgumentNullException(nameof(group));

        if (group.Id == default)
        {
            group.Id = Server.GetNewGroupId();
        }

        base.Add(group);
    }
}
