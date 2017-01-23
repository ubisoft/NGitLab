using System.Runtime.Serialization;

namespace NGitLab.Models
{
    /// <summary>
    /// Allows to use more advanced gitlab queries for getting projects.
    /// </summary>
    [DataContract]
    public class ProjectQuery
    {
        public ProjectQueryScope Scope = ProjectQueryScope.Accessible;

        /// <summary>
        /// Limit by archived status
        /// </summary>
        public bool? Archived;

        /// <summary>
        /// Limit by visibility public, internal, or private
        /// </summary>
        public VisibilityLevel? Visibility;

        /// <summary>
        /// Return projects ordered by id, name, path, created_at, updated_at, or last_activity_at fields. Default is created_at
        /// </summary>
        public string OrderBy;

        /// <summary>
        /// Return list of authorized projects matching the search criteria
        /// </summary>
        public string Search;

        /// <summary>
        /// Return list of authorized projects matching the search criteria
        /// </summary>
        public bool? Ascending;

        /// <summary>
        /// Return only the ID, URL, name, and path of each project
        /// </summary>
        public bool? Simple;
    }

    public enum ProjectQueryScope
    {
        /// <summary>
        /// Get a list of projects for which the authenticated user is a member.
        /// </summary>
        Accessible,
        /// <summary>
        /// Get a list of projects owned by the authenticated user.
        /// </summary>
        Owned,
        /// <summary>
        /// Get a list of projects which the authenticated user can see.
        /// </summary>
        Visible,
        /// <summary>
        /// Get a list of all GitLab projects (admin only).
        /// </summary>
        All
    }
}