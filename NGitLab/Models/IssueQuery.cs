using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced GitLab queries for getting issues
    /// </summary>
    public class IssueQuery
    {
        /// <summary>
        /// Return all issues or just those that are open, closed, locked, or merged
        /// </summary>
        [QueryParameter("state")]
        public IssueState? State { get; set; }

        /// <summary>
        /// Return all issues or just those that are of type issue, incident or test_case
        /// </summary>
        [QueryParameter("issue_type")]
        public IssueType? Type { get; set; }

        /// <summary>
        /// Return requests ordered by created_at or updated_at fields. Default is created_at
        /// </summary>
        [QueryParameter("order_by")]
        public string OrderBy { get; set; }

        /// <summary>
        /// Return requests sorted in asc or desc order. Default is desc
        /// </summary>
        [QueryParameter("sort")]
        public string Sort { get; set; }

        /// <summary>
        /// Return issues for a specific milestone
        /// </summary>
        [QueryParameter("milestone")]
        public string Milestone { get; set; }

        /// <summary>
        /// Return issues matching a comma separated list of labels
        /// </summary>
        [QueryParameter("labels")]
        public string Labels { get; set; }

        /// <summary>
        /// Return issues created on or after the given time
        /// </summary>
        [QueryParameter("created_after")]
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// Return issues created on or before the given time
        /// </summary>
        [QueryParameter("created_before")]
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// Return issues updated on or after the given time
        /// </summary>
        [QueryParameter("updated_after")]
        public DateTime? UpdatedAfter { get; set; }

        /// <summary>
        /// Return issues updated on or before the given time
        /// </summary>
        [QueryParameter("updated_before")]
        public DateTime? UpdatedBefore { get; set; }

        /// <summary>
        /// Return issues that are flagged as confidential
        /// </summary>
        [QueryParameter("confidential")]
        public bool? Confidential { get; set; }

        /// <summary>
        /// Return issues for the given scope: created_by_me, assigned_to_me or all. Defaults to created_by_me
        /// </summary>
        [QueryParameter("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Returns issues created by the given user id. Combine with scope=all or scope=assigned_to_me
        /// </summary>
        [QueryParameter("author_id")]
        public int? AuthorId { get; set; }

        /// <summary>
        /// Returns issues assigned to the given user id
        /// </summary>
        [QueryParameter("assignee_id")]
        public QueryAssigneeId AssigneeId { get; set; }

        /// <summary>
        /// Search issues against their title and description
        /// </summary>
        [QueryParameter("search")]
        public string Search { get; set; }

        /// <summary>
        /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
        /// </summary>
        [QueryParameter("per_page")]
        public int? PerPage { get; set; }
    }
}
