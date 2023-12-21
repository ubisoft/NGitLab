using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock.Config;

public class GitLabJob : GitLabObject<GitLabPipeline>
{
    public string Name { get; set; }

    public string Stage { get; set; }

    public JobStatus Status { get; set; }

    public bool AllowFailure { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public string[] TagList { get; set; }

    public GitLabPipeline DownstreamPipeline { get; set; }
}

public class GitLabJobsCollection : GitLabCollection<GitLabJob, GitLabPipeline>
{
    internal GitLabJobsCollection(GitLabPipeline parent)
        : base(parent)
    {
    }

    internal override void SetItem(GitLabJob item)
    {
        if (item == null)
            return;

        item.Parent = (GitLabPipeline)_parent;

        if (item.Id == default)
            item.Id = GetAllJobs().Select(x => x.Id).DefaultIfEmpty().Max() + 1;
    }

    private IEnumerable<GitLabJob> GetAllJobs()
    {
        var config = _parent switch
        {
            GitLabProject project => project.Parent,
            GitLabPipeline pipeline => pipeline.Parent?.Parent,
            _ => null,
        };

        return config == null
            ? Enumerable.Empty<GitLabJob>()
            : config.Projects.SelectMany(x => x.Pipelines.SelectMany(y => y.Jobs));
    }
}
