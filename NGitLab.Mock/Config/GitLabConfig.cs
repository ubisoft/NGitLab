namespace NGitLab.Mock.Config
{
    /// <summary>
    /// Describe content of a GitLab server
    /// </summary>
    public class GitLabConfig
    {
        public GitLabConfig()
        {
            Users = new GitLabUsersCollection(this);
            Groups = new GitLabGroupsCollection(this);
            Projects = new GitLabProjectsCollection(this);
        }

        /// <summary>
        /// User used by default when not specified in extensions methods
        /// </summary>
        public string DefaultUser { get; set; }

        public GitLabUsersCollection Users { get; }

        /// <summary>
        /// Explicit groups
        /// </summary>
        public GitLabGroupsCollection Groups { get; }

        public GitLabProjectsCollection Projects { get; }

        /// <summary>
        /// Serialize config to YAML format
        /// </summary>
        public string Serialize()
        {
            return ConfigSerializer.Serialize(this);
        }

        /// <summary>
        /// Deserialize YAML content to config
        /// </summary>
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
