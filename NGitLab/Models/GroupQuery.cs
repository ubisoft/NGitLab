using System.Runtime.Serialization;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced gitlab queries for getting groups (based on v4 Gitlab API).
    /// https://docs.gitlab.com/ee/api/groups.html
    /// </summary>
    [DataContract]
    public class GroupQuery
    {
        /// <summary>
        /// Skip the group IDs passed
        /// (optional)
        /// </summary>
        public int[] SkipGroups;

        /// <summary>
        /// Show all the groups you have access to (defaults to false for authenticated users, true for admin); Attributes owned and min_access_level have precedence
        /// (optional)
        /// </summary>
        public bool? AllAvailable;

        /// <summary>
        /// Return the list of authorized groups matching the search criteria
        /// (optional)
        /// </summary>
        public string Search;

        /// <summary>
        /// Order groups by name, path or id. Default is name
        /// (optional)
        /// </summary>
        public string OrderBy;

        /// <summary>
        /// Order groups in asc or desc order. Default is ascending
        /// (optional)
        /// </summary>
        public string Sort;

        /// <summary>
        /// Include group statistics (admins only)
        /// (optional)
        /// </summary>
        public bool? Statistics;

        /// <summary>
        /// Include custom attributes in response (admins only)
        /// (optional)
        /// </summary>
        public bool? WithCustomAttributes;

        /// <summary>
        /// Limit to groups explicitly owned by the current user
        /// (optional)
        /// </summary>
        public bool? Owned;

        /// <summary>
        /// Limit to groups where current user has at least this access level
        /// (optional)
        /// </summary>
        public AccessLevel? MinAccessLevel;
    }
}
