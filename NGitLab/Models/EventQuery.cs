using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced GitLab queries for getting issues
    /// </summary>
    public class EventQuery
    {
        /// <summary>
        /// Include only events of a particular action type
        /// </summary>
        [QueryParameter("action")]
        public EventAction? Action { get; set; }

        /// <summary>
        /// Include only events of a particular target type
        /// </summary>
        [QueryParameter("target_type")]
        public EventTargetType? Type { get; set; }

        /// <summary>
        /// Include only events created before a particular date.
        /// </summary>
        public DateTime? Before { get; set; }

        [QueryParameter("before")]
        public DateTime? ActualBefore => Before?.Date;

        /// <summary>
        /// Include only events created after a particular date.
        /// </summary>
        public DateTime? After { get; set; }

        [QueryParameter("after")]
        public DateTime? ActualAfter => After?.Date;

        /// <summary>
        /// Include all events across a user’s projects.
        /// </summary>
        [QueryParameter("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Sort events in asc or desc order by created_at. Default is desc
        /// </summary>
        [QueryParameter("sort")]
        public string Sort { get; set; }

        /// <summary>
        /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
        /// </summary>
        [QueryParameter("per_page")]
        public int? PerPage { get; set; }
    }
}
