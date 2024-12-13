using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IGroupBadgeClient
{
    IEnumerable<Badge> All { get; }

    Badge this[long id] { get; }

    Badge Create(BadgeCreate badge);

    Badge Update(long id, BadgeUpdate badge);

    void Delete(long id);
}
