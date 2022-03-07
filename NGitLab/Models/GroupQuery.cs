using System.Runtime.Serialization;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced GitLab queries for getting groups (based on v4 GitLab API).
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

    /// <summary>
    /// Allows to use more advanced GitLab queries for getting project IDs (based on v4 GitLab API).
    /// https://docs.gitlab.com/ee/api/groups.html
    /// </summary>
    [DataContract]
    public class SearchProjectQuery
    {
        /// <summary>
        /// Specifiy group id to look into as string
        /// </summary>
        public string GroupId;

        /// <summary>
        /// Specify project name to search for as string
        /// </summary>
        public string Search;

        /// <summary>
        /// Specify project scope defined by GroupQueryScope
        /// </summary>
        public GroupQueryScope Scope;
    }

    // implementation according to https://docs.gitlab.com/ee/api/search.html#scope-projects-1
    public enum GroupQueryScope
    {
        /// <summary>
        /// The response depends on the requested scope project
        /// </summary>
        Projects,

        /// <summary>
        /// The response depends on the requested scope issues
        /// </summary>
        Issues,

        /// <summary>
        /// The response depends on the requested scope merge_requests
        /// </summary>
        Merge_requests,

        /// <summary>
        /// The response depends on the requested scope milestones
        /// </summary>
        Milestones,

        /// <summary>
        /// The response depends on the requested scope wiki_blobs
        /// </summary>
        Wiki_blobs,

        /// <summary>
        /// The response depends on the requested scope commits
        /// </summary>
        Commits,

        /// <summary>
        /// The response depends on the requested scope blobs
        /// </summary>
        Blobs,

        /// <summary>
        /// The response depends on the requested scope notes
        /// </summary>
        Notes,

        /// <summary>
        /// The response depends on the requested scope users
        /// </summary>
        Users,
    }
}
