using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced GitLab queries for getting projects.
    /// </summary>
    public class ForkedProjectQuery
    {
        /// <summary>
        /// Limit by archived status
        /// </summary>
        [QueryParameter("archived")]
        public bool? Archived;

        /// <summary>
        /// Limit by visibility public, internal, or private
        /// </summary>
        public VisibilityLevel? Visibility;

        [QueryParameter("visibility")]
        public string ActualVisibility => Visibility.HasValue ? Visibility.ToString().ToLowerInvariant() : null;

        /// <summary>
        /// Return projects ordered by id, name, path, created_at, updated_at, or last_activity_at fields. Default is created_at
        /// </summary>
        public string OrderBy;

        [QueryParameter("order_by")]
        public string ActualOrderBy => string.IsNullOrEmpty(OrderBy) ? "id" : OrderBy;

        /// <summary>
        /// Return list of authorized projects matching the search criteria
        /// </summary>
        [QueryParameter("search")]
        public string Search;

        /// <summary>
        /// Return only the ID, URL, name, and path of each project
        /// </summary>
        [QueryParameter("simple")]
        public bool? Simple;

        /// <summary>
        /// Include project statistics
        /// </summary>
        [QueryParameter("statistics")]
        public bool? Statistics;

        /// <summary>
        /// Limit by projects explicitly owned by the current user
        /// </summary>
        [QueryParameter("owned")]
        public bool? Owned;

        /// <summary>
        /// Limit by projects that the current user is a member of
        /// </summary>
        [QueryParameter("membership")]
        public bool? Membership;

        /// <summary>
        /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
        /// </summary>
        [QueryParameter("per_page")]
        public int? PerPage;

        /// <summary>
        /// Limit to projects where current user has at least this access level
        /// (optional)
        /// </summary>
        public AccessLevel? MinAccessLevel;

        [QueryParameter("min_access_level")]
        public int? ActualMinAccessLevel => (int?)MinAccessLevel;

        [QueryParameter("pagination")]
        public string Pagination => string.Equals(ActualOrderBy, "id", StringComparison.Ordinal) ? "keyset" : null;
    }
}
