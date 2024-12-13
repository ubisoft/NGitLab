using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class PipelineCollection : Collection<Pipeline>
{
    private readonly Project _project;

    public PipelineCollection(GitLabObject parent)
        : base(parent)
    {
        _project = parent as Project ??
                   throw new ArgumentException("Parent must be a Project", nameof(parent));
    }

    public Pipeline GetById(long id)
    {
        return this.FirstOrDefault(pipeline => pipeline.Id == id);
    }

    public override void Add(Pipeline pipeline)
    {
        if (pipeline is null)
            throw new ArgumentNullException(nameof(pipeline));

        if (pipeline.Id == default)
        {
            pipeline.Id = Server.GetNewPipelineId();
        }

        // Check if pipeline.Ref represents a branch or a tag
        var branch = _project.Repository.GetAllBranches()
            .FirstOrDefault(b => string.Equals(pipeline.Ref, b.FriendlyName, StringComparison.Ordinal));
        if (branch is not null)
        {
            pipeline.Sha = new Sha1(branch.Tip.Sha);
        }
        else
        {
            var commit = _project.Repository.GetCommit(pipeline.Ref);
            if (commit is not null)
                pipeline.Sha = new Sha1(commit.Sha);
        }

        base.Add(pipeline);
    }

    public Pipeline Add(string @ref, JobStatus status, User user)
    {
        var pipeline = new Pipeline(@ref)
        {
            Status = status,
            User = user,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        Add(pipeline);
        return pipeline;
    }
}
