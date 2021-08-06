namespace NGitLab.Mock.Config
{
    public class GitLabConfig
    {
        public GitLabConfig()
        {
            Users = new GitLabUsersCollection(this);
            Groups = new GitLabGroupsCollection(this);
            Projects = new GitLabProjectsCollection(this);
        }

        public string DefaultUser { get; set; }

        public GitLabUsersCollection Users { get; }

        public GitLabGroupsCollection Groups { get; }

        public GitLabProjectsCollection Projects { get; }

        public string Serialize()
        {
            return ConfigSerializer.Serialize(this);
        }

        public static GitLabConfig Deserialize(string content)
        {
            var serializer = new ConfigSerializer();
            serializer.Deserialize(content);
            var config = new GitLabConfig();
            return serializer.TryGet("gitlab", ref config)
                ? config
                : throw new GitLabException("Cannot deserialize YAML config");
        }
    }
}
