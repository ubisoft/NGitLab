namespace NGitLab.Mock.Fluent
{
    public class GitLabUser : GitLabObject<GitLabConfig>
    {
        public string Username { get; set; }

        public string Name { get; set; } = "User";

        public string Email { get; set; } = "user@ubisoft.com";

        public string AvatarUrl { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class GitLabUsersCollection : GitLabCollection<GitLabUser, GitLabConfig>
    {
        internal GitLabUsersCollection(GitLabConfig parent)
            : base(parent)
        {
        }
    }
}
