using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock.Config;

/// <summary>
/// Describe a milestone in a GitLab project
/// </summary>
public class GitLabMilestone : GitLabObject
{
    /// <summary>
    /// Title (required)
    /// </summary>
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }
}

public class GitLabMilestonesCollection : GitLabCollection<GitLabMilestone>
{
    internal GitLabMilestonesCollection(GitLabProject parent)
        : base(parent)
    {
    }

    internal GitLabMilestonesCollection(GitLabGroup parent)
        : base(parent)
    {
    }

    internal override void SetItem(GitLabMilestone item)
    {
        if (item == null)
            return;

        item.ParentObject = _parent;

        if (item.Id == default)
            item.Id = GetAllMilestones().Select(x => x.Id).DefaultIfEmpty().Max() + 1;
    }

    private IEnumerable<GitLabMilestone> GetAllMilestones()
    {
        var config = _parent switch
        {
            GitLabProject project => project.Parent,
            _ => null,
        };

        return config == null
            ? Enumerable.Empty<GitLabMilestone>()
            : config.Projects.SelectMany(x => x.Milestones);
    }
}
