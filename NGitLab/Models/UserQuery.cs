namespace NGitLab
{
    public class UserQuery
    {
        /// <summary>
        /// Specifies how many record per paging (Gitlab supports a maximum of 100 projects and defaults to 20).
        /// </summary>
        public int? PerPage;

        /// <summary>
        /// Get users that match the search query
        /// </summary>
        public string Search;

        /// <summary>
        /// Get users that match the username
        /// </summary>
        public string Username;

        /// <summary>
        /// If true, get only users that are active
        /// </summary>
        public bool? IsActive;

        /// <summary>
        /// If true, get only users that are blocked
        /// </summary>
        public bool? IsBlocked;

        /// <summary>
        /// If true, get only users that are external
        /// </summary>
        public bool? IsExternal;

        /// <summary>
        /// Exclude the extenral users
        /// </summary>
        public bool? ExcludeExternal;

        /// <summary>
        /// (Admin only) Return projects ordered by id, name, username, created_at, or updated_at. Default is id.
        /// </summary>
        public string OrderBy;

        /// <summary>
        /// (Admin only) Order users in asc or desc order. Default is descending
        /// </summary>
        public string Sort;
    }
}
