using System.Collections.Generic;
using System.Globalization;

namespace NGitLab.Mock
{
    public static class ProjectExtensions
    {
        public static Project FindProject(this IEnumerable<Project> projects, string idOrPathWithNamespace)
        {
            foreach (var project in projects)
            {
                if (project.Id.ToString(CultureInfo.InvariantCulture) == idOrPathWithNamespace)
                    return project;
            }

            foreach (var project in projects)
            {
                if (project.PathWithNamespace == idOrPathWithNamespace)
                    return project;
            }

            return null;
        }

        public static Project FindById(this IEnumerable<Project> projects, int id)
        {
            foreach (var project in projects)
            {
                if (project.Id == id)
                    return project;
            }

            return null;
        }
    }
}
