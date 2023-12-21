using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class ProjectBadgeClient : BadgeClient, IProjectBadgeClient
{
    public ProjectBadgeClient(API api, ProjectId projectId)
        : base(api, $"{Project.Url}/{projectId.ValueAsUriParameter()}")
    {
    }

    /// <inheritdoc/>
    public IEnumerable<Badge> ProjectsOnly => All.Where(b => b.Kind == BadgeKind.Project);
}
