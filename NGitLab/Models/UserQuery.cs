namespace NGitLab
{
    public class UserQuery
    {
        /// <summary>
        /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
        /// </summary>
        public int? PerPage { get; set; }

        /// <summary>
        /// Get users that match the search query
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Get users that match the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// If true, get only users that are active
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// If true, get only users that are blocked
        /// </summary>
        public bool? IsBlocked { get; set; }

        /// <summary>
        /// If true, get only users that are external
        /// </summary>
        public bool? IsExternal { get; set; }

        /// <summary>
        /// Exclude external users
        /// </summary>
        public bool? ExcludeExternal { get; set; }

        /// <summary>
        /// (Admin only) Return projects ordered by id, name, username, created_at, or updated_at. Default is id.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// (Admin only) Order users in asc or desc order. Default is descending
        /// </summary>
        public string Sort { get; set; }
    }
}
