using System.Collections.Generic;
using System.Globalization;

namespace NGitLab.Mock
{
    public static class GroupExtensions
    {
        public static Group FindGroupById(this IEnumerable<Group> groups, int id)
        {
            foreach (var group in groups)
            {
                if (group.Id == id)
                    return group;
            }

            return null;
        }

        public static Group FindGroup(this IEnumerable<Group> groups, string idOrPathWithNamespace)
        {
            foreach (var group in groups)
            {
                if (group.PathWithNameSpace == idOrPathWithNamespace)
                    return group;

                if (group.Id.ToString(CultureInfo.InvariantCulture) == idOrPathWithNamespace)
                    return group;
            }

            return null;
        }
    }
}
