using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock.Config;

public class GitLabPipeline : GitLabObject<GitLabProject>
{
    public GitLabPipeline()
    {
        Jobs = new GitLabJobsCollection(this);
    }

    public GitLabPipeline(GitLabProject project)
    {
        Parent = project;
        Jobs = new GitLabJobsCollection(this);
    }

    /// <summary>
    /// Commit alias reference
    /// </summary>
    public string Commit { get; set; }

    public GitLabJobsCollection Jobs { get; }
}

public class GitLabPipelinesCollection : GitLabCollection<GitLabPipeline, GitLabProject>
{
    internal GitLabPipelinesCollection(GitLabProject parent)
        : base(parent)
    {
    }

    internal override void SetItem(GitLabPipeline item)
    {
        if (item == null)
            return;

        item.Parent = (GitLabProject)_parent;

        if (item.Id == default)
            item.Id = GetAllPipelines().Select(x => x.Id).DefaultIfEmpty().Max() + 1;
    }

    private IEnumerable<GitLabPipeline> GetAllPipelines()
    {
        var config = _parent switch
        {
            GitLabProject project => project.Parent,
            _ => null,
        };

        return config == null
            ? Enumerable.Empty<GitLabPipeline>()
            : config.Projects.SelectMany(x => x.Pipelines);
    }
}
