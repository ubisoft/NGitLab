using NGitLab.Models;

namespace NGitLab.Mock.Config
{
    public class GitLabPermission : GitLabObject
    {
        public string User { get; set; }

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
