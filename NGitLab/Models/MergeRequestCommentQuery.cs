using System.Runtime.Serialization;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced gitlab queries for getting projects.
    /// </summary>
    [DataContract]
    public class MergeRequestCommentQuery
    {
        /// <summary>
        /// Specifies how many record per paging (Gitlab supports a maximum of 100 projects and defaults to 20).
        /// </summary>
        public int? PerPage { get; set; }

        /// <summary>
        /// Specifies the specific page desired.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Specifies if the sorting is ascending or descending
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Specifies the field the sorting should be done on (created_at or updated_at)
        /// </summary>
        public string OrderBy { get; set; }
    }
}
