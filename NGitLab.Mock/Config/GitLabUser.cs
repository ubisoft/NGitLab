namespace NGitLab.Mock.Config
{
    public class GitLabUser : GitLabObject<GitLabConfig>
    {
        public string Username { get; set; }

        public string Name { get; set; } = "User";

        public string Email { get; set; } = "user@example.com";

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
