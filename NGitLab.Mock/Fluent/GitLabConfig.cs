namespace NGitLab.Mock.Fluent
{
    public class GitLabConfig
    {
        public GitLabConfig()
        {
            Users = new GitLabUsersCollection(this);
            Projects = new GitLabProjectsCollection(this);
        }

        public string CurrentUser { get; set; }

        public GitLabUsersCollection Users { get; }

        public GitLabProjectsCollection Projects { get; }
    }
}
