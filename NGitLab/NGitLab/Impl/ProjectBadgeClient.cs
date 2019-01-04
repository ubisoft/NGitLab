using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    internal class ProjectBadgeClient : BadgeClient, IProjectBadgeClient
    {
        public ProjectBadgeClient(API api, int projectId)
            : base(api, Project.Url + $"/{projectId}")
        {
        }

        /// <inheritdoc/>
        public IEnumerable<Badge> ProjectsOnly => All.Where(b => b.Kind == BadgeKind.Project);
    }
}
