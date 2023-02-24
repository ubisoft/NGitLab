using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced GitLab queries for getting merge requests
    /// </summary>
    public class MergeRequestQuery
    {
        /// <summary>
        /// Return all merge requests or just those that are opened, closed, locked, or merged
        /// </summary>
        [QueryParameter("state")]
        public MergeRequestState? State { get; set; }

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
        /// Return merge requests for a specific milestone
        /// </summary>
        [QueryParameter("milestone")]
        public string Milestone { get; set; }

        /// <summary>
        /// If simple, returns the iid, URL, title, description, and basic state of merge request
        /// </summary>
        [QueryParameter("view")]
        public string View { get; set; }

        /// <summary>
        /// Return merge requests matching a comma separated list of labels
        /// </summary>
        [QueryParameter("labels")]
        public string Labels { get; set; }

        /// <summary>
        /// Return merge requests created on or after the given time
        /// </summary>
        [QueryParameter("created_after")]
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// Return merge requests created on or before the given time
        /// </summary>
        [QueryParameter("created_before")]
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// Return merge requests updated on or after the given time
        /// </summary>
        [QueryParameter("updated_after")]
        public DateTime? UpdatedAfter { get; set; }

        /// <summary>
        /// Return merge requests updated on or before the given time
        /// </summary>
        [QueryParameter("updated_before")]
        public DateTime? UpdatedBefore { get; set; }

        /// <summary>
        /// Return merge requests for the given scope: created_by_me, assigned_to_me or all. Defaults to created_by_me
        /// </summary>
        [QueryParameter("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Returns merge requests created by the given user id. Combine with scope=all or scope=assigned_to_me
        /// </summary>
        [QueryParameter("author_id")]
        public int? AuthorId { get; set; }

        /// <summary>
        /// Returns merge requests assigned to the given user id
        /// </summary>
        [QueryParameter("assignee_id")]
        public QueryAssigneeId AssigneeId { get; set; }

        /// <summary>
        /// Returns the merge requests reviewer ids.
        /// </summary>
        [QueryParameter("reviewer_id")]
        public QueryAssigneeId ReviewerId { get; set; }

        /// <summary>
        /// Returns merge requests which have specified all the users with the given ids as individual approvers.
        /// None returns merge requests without approvers. Any returns merge requests with an approver.
        /// </summary>
        [QueryParameter("approver_ids[]")]
        public int[] ApproverIds { get; set; }

        /// <summary>
        /// Return merge requests with the given source branch
        /// </summary>
        [QueryParameter("source_branch")]
        public string SourceBranch { get; set; }

        /// <summary>
        /// Return merge requests with the given target branch
        /// </summary>
        [QueryParameter("target_branch")]
        public string TargetBranch { get; set; }

        /// <summary>
        /// Search merge requests against their title and description
        /// </summary>
        [QueryParameter("search")]
        public string Search { get; set; }

        /// <summary>
        /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
        /// </summary>
        [QueryParameter("per_page")]
        public int? PerPage { get; set; }

        /// <summary>
        /// Filter merge requests against their wip status. yes to return only WIP merge requests, no to return non WIP merge requests
        /// </summary>
        public bool? Wip { get; set; }

        [QueryParameter("wip")]
        public string ActualWip => !Wip.HasValue ? null : (Wip.Value ? "yes" : "no");
    }
}
