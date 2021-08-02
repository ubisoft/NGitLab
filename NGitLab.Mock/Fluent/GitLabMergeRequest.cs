using System;
using System.Collections.Generic;

namespace NGitLab.Mock.Fluent
{
    public class GitLabMergeRequest : GitLabObject<GitLabProject>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Assignee { get; set; }

        public string SourceBranch { get; set; }

        public string TargetBranch { get; set; } = "main";

        public IList<string> Labels { get; } = new List<string>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? MergedAt { get; set; }

        public DateTime? ClosedAt { get; set; }
    }

    public class GitLabMergeRequestsCollection : GitLabCollection<GitLabMergeRequest, GitLabProject>
    {
        internal GitLabMergeRequestsCollection(GitLabProject parent)
            : base(parent)
        {
        }
    }
}
