using NGitLab.Models;

namespace NGitLab.Mock.Fluent
{
    public class GitLabPermission : GitLabObject<GitLabProject>
    {
        public string User { get; set; }

        public string Group { get; set; }

        public AccessLevel Level { get; set; } = AccessLevel.Developer;
    }

    public class GitLabPermissionsCollection : GitLabCollection<GitLabPermission, GitLabProject>
    {
        internal GitLabPermissionsCollection(GitLabProject parent)
            : base(parent)
        {
        }
    }
}
