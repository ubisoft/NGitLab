using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock;

public static class ProjectExtensions
{
    public static Project FindById(this IEnumerable<Project> projects, long id)
    {
        return projects.FirstOrDefault(project => project.Id == id);
    }

    public static Project FindByNamespacedPath(this IEnumerable<Project> projects, string pathWithNamespace)
    {
        return projects.FirstOrDefault(project => string.Equals(project.PathWithNamespace, pathWithNamespace, StringComparison.OrdinalIgnoreCase));
    }

    public static Project FindProject(this IEnumerable<Project> projects, string idOrPathWithNamespace)
    {
        return long.TryParse(idOrPathWithNamespace, out var id)
            ? FindById(projects, id)
            : FindByNamespacedPath(projects, idOrPathWithNamespace);
    }
}
