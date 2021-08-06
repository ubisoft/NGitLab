using System.Collections.Generic;

namespace NGitLab.Mock.Config
{
    public class GitLabCommit : GitLabObject<GitLabProject>
    {
        public string User { get; set; }

        public string Message { get; set; }

        public IList<GitLabFileDescriptor> Files { get; } = new List<GitLabFileDescriptor>();

        public IList<string> Tags { get; } = new List<string>();

        public string SourceBranch { get; set; }

        public string TargetBranch { get; set; }
    }

    public class GitLabCommitsCollection : GitLabCollection<GitLabCommit, GitLabProject>
    {
        internal GitLabCommitsCollection(GitLabProject parent)
            : base(parent)
        {
        }
    }
}
