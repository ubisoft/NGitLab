using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock;

public static class GroupExtensions
{
    public static Group FindById(this IEnumerable<Group> groups, long id)
    {
        return groups.FirstOrDefault(group => group.Id == id);
    }

    public static Group FindByNamespacedPath(this IEnumerable<Group> groups, string pathWithNamespace)
    {
        return groups.FirstOrDefault(group => string.Equals(group.PathWithNameSpace, pathWithNamespace, StringComparison.Ordinal));
    }

    public static Group FindGroup(this IEnumerable<Group> groups, string idOrPathWithNamespace)
    {
        return long.TryParse(idOrPathWithNamespace, out var id)
            ? FindById(groups, id)
            : FindByNamespacedPath(groups, idOrPathWithNamespace);
    }
}
