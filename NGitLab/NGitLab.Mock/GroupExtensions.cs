using System.Collections.Generic;
using System.Globalization;

namespace NGitLab.Mock
{
    public static class GroupExtensions
    {
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
