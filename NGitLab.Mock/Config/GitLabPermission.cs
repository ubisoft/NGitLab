using NGitLab.Models;

namespace NGitLab.Mock.Config
{
    /// <summary>
    /// Describe a permission in a GitLab group/project
    /// </summary>
    public class GitLabPermission : GitLabObject
    {
        /// <summary>
        /// Username (required if user permission)
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Group fullname (required is group permission)
        /// </summary>
        public string Group { get; set; }

        public AccessLevel Level { get; set; } = AccessLevel.Developer;
    }

    public class GitLabPermissionsCollection : GitLabCollection<GitLabPermission>
    {
        internal GitLabPermissionsCollection(GitLabProject parent)
            : base(parent)
        {
        }

        internal GitLabPermissionsCollection(GitLabGroup parent)
            : base(parent)
        {
        }
    }
}
