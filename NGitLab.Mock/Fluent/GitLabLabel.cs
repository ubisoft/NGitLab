namespace NGitLab.Mock.Fluent
{
    public class GitLabLabel : GitLabObject<GitLabProject>
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public bool Group { get; set; }
    }

    public class GitLabLabelsCollection : GitLabCollection<GitLabLabel, GitLabProject>
    {
        internal GitLabLabelsCollection(GitLabProject parent)
            : base(parent)
        {
        }
    }
}
