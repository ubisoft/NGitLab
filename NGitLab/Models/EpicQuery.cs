using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced GitLab queries for getting deployments
    /// </summary>
    public class EpicQuery
    {
        /// <summary>
        /// Return all issues or just those that are open, closed
        /// </summary>
        [QueryParameter("state")]
        public EpicState? State { get; set; }

        /// <summary>
        /// Return requests ordered by id, iid, created_at or updated_at fields. Default is id
        /// </summary>
        [QueryParameter("order_by")]
        public string OrderBy { get; set; }

        /// <summary>
        /// Return requests sorted in asc or desc order. Default is asc
        /// </summary>
        [QueryParameter("sort")]
        public string Sort { get; set; }

        /// <summary>
        /// Return epics matching a comma separated list of labels
        /// </summary>
        [QueryParameter("labels")]
        public string Labels { get; set; }

        /// <summary>
        /// Return epics created on or after the given time
        /// </summary>
        [QueryParameter("created_after")]
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// Return epics created on or before the given time
        /// </summary>
        [QueryParameter("created_before")]
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// Return epics updated on or after the given time
        /// </summary>
        [QueryParameter("updated_after")]
        public DateTime? UpdatedAfter { get; set; }

        /// <summary>
        /// Return epics updated on or before the given time
        /// </summary>
        [QueryParameter("updated_before")]
        public DateTime? UpdatedBefore { get; set; }

        /// <summary>
        /// Search issues against their title and description
        /// </summary>
        [QueryParameter("search")]
        public string Search { get; set; }
    }
}
