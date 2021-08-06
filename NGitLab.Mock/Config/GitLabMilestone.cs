using System;

namespace NGitLab.Mock.Config
{
    public class GitLabMilestone : GitLabObject
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? ClosedAt { get; set; }
    }

    public class GitLabMilestonesCollection : GitLabCollection<GitLabMilestone>
    {
        internal GitLabMilestonesCollection(GitLabProject parent)
            : base(parent)
        {
        }

        internal override void SetItem(GitLabMilestone item)
        {
            if (item == null)
                return;

            item.Parent = _parent;
        }
    }
}
