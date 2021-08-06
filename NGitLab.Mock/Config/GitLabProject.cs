using NGitLab.Models;

namespace NGitLab.Mock.Config
{
    public class GitLabProject : GitLabObject<GitLabConfig>
    {
        public GitLabProject()
        {
            Commits = new GitLabCommitsCollection(this);
            Issues = new GitLabIssuesCollection(this);
            MergeRequests = new GitLabMergeRequestsCollection(this);
            Permissions = new GitLabPermissionsCollection(this);
            Labels = new GitLabLabelsCollection(this);
        }

        public string Name { get; set; }

        public string Namespace { get; set; } = "functional";

        public string DefaultBranch { get; set; } = "main";

        public string Description { get; set; }

        public string ClonePath { get; set; }

        public VisibilityLevel Visibility { get; set; } = VisibilityLevel.Internal;

        public GitLabCommitsCollection Commits { get; }

        public GitLabIssuesCollection Issues { get; }

        public GitLabMergeRequestsCollection MergeRequests { get; }

        public GitLabPermissionsCollection Permissions { get; }

        public GitLabLabelsCollection Labels { get; }
    }

    public class GitLabProjectsCollection : GitLabCollection<GitLabProject, GitLabConfig>
    {
        internal GitLabProjectsCollection(GitLabConfig parent)
            : base(parent)
        {
        }
    }
}
