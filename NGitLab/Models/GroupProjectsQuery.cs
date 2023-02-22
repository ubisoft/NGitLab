using System;

namespace NGitLab.Models
{
    public sealed class GroupProjectsQuery
    {
        /// <summary>
        /// Limit by archived status
        /// </summary>
        [QueryParameter("archived")]
        public bool? Archived { get; set; }

        /// <summary>
        /// Limit by visibility
        /// </summary>
        public VisibilityLevel? Visibility { get; set; }

        [QueryParameter("visibility")]
        public string ActualVisibility => Visibility?.ToString().ToLowerInvariant();

        /// <summary>
        /// Return projects ordered by id, name, path, created_at, updated_at, similarity, or last_activity_at fields. Default is created_at
        /// </summary>
        public string OrderBy { get; set; }

        [QueryParameter("order_by")]
        public string ActualOrderBy => string.IsNullOrEmpty(OrderBy) ? "id" : OrderBy;

        /// <summary>
        /// Return projects sorted in asc or desc order. Default is desc
        /// </summary>
        [QueryParameter("sort")]
        public string Sort { get; set; }

        /// <summary>
        /// Return list of authorized projects matching the search criteria
        /// </summary>
        [QueryParameter("search")]
        public string Search { get; set; }

        /// <summary>
        /// Return only the ID, URL, name, and path of each project
        /// </summary>
        [QueryParameter("simple")]
        public bool? Simple { get; set; }

        /// <summary>
        /// Limit by projects owned by the current user
        /// </summary>
        [QueryParameter("owned")]
        public bool? Owned { get; set; }

        /// <summary>
        /// Limit by projects starred by the current user
        /// </summary>
        [QueryParameter("starred")]
        public bool? Starred { get; set; }

        /// <summary>
        /// Limit by projects with issues feature enabled. Default is false
        /// </summary>
        [QueryParameter("with_issues_enabled")]
        public bool? WithIssuesEnabled { get; set; }

        /// <summary>
        /// Limit by projects with merge requests feature enabled. Default is false
        /// </summary>
        [QueryParameter("with_merge_requests_enabled")]
        public bool? WithMergeRequestsEnabled { get; set; }

        /// <summary>
        /// Include projects shared to this group. Default is true
        /// </summary>
        [QueryParameter("with_shared")]
        public bool? WithShared { get; set; }

        /// <summary>
        /// Include projects in subgroups of this group. Default is false
        /// </summary>
        [QueryParameter("include_subgroups")]
        public bool? IncludeSubGroups { get; set; }

        /// <summary>
        /// Limit to projects where current user has at least this access level
        /// </summary>
        public AccessLevel? MinAccessLevel { get; set; }

        [QueryParameter("min_access_level")]
        public int? ActualMinAccessLevel => (int?)MinAccessLevel;

        /// <summary>
        /// Include custom attributes in response (administrators only)
        /// </summary>
        [QueryParameter("with_custom_attributes")]
        public bool? WithCustomAttributes { get; set; }

        /// <summary>
        /// Return only projects that have security reports artifacts present in any of their builds. This means “projects with security reports enabled”. Default is false
        /// </summary>
        [QueryParameter("with_security_reports")]
        public bool? WithSecurityReports { get; set; }

        [QueryParameter("pagination")]
        public string Pagination => string.Equals(ActualOrderBy, "id", StringComparison.Ordinal) ? "keyset" : null;
    }
}
