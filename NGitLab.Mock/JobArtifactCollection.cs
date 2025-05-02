using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class JobArtifactCollection(GitLabObject container)
    : Collection<JobArtifact>(container)
{
    public override void Add(JobArtifact artifact)
    {
        if (artifact is null)
            throw new ArgumentNullException(nameof(artifact));

        if (string.IsNullOrEmpty(artifact.Filename))
            throw new ArgumentException("Filename must be set", nameof(artifact));

        if (artifact.Size < 1)
            throw new ArgumentException("Size must be set", nameof(artifact));

        if (this.Any(g => StringComparer.Ordinal.Equals(g.Filename, artifact.Filename)))
            throw new NotSupportedException($"Artifact '{artifact.Filename}' already added");

        base.Add(artifact);
    }

    public new void Clear()
    {
        foreach (var item in this)
        {
            item.Parent = null;
        }

        base.Clear();
    }
}
