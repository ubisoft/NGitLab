namespace NGitLab.Mock.Config
{
    public class GitLabLabel : GitLabObject
    {
        public string Name { get; set; }

        public string Color { get; set; } = "#d9534f";

        public string Description { get; set; }
    }

    public class GitLabLabelsCollection : GitLabCollection<GitLabLabel>
    {
        internal GitLabLabelsCollection(GitLabProject parent)
            : base(parent)
        {
        }

        internal GitLabLabelsCollection(GitLabGroup parent)
            : base(parent)
        {
        }
    }
}
