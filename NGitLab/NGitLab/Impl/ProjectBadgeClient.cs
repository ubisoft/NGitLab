using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{

    internal class ProjectBadgeClient : BadgeClient, IProjectBadgeClient
    {
        public ProjectBadgeClient(API api, int projectId)
            : base(api, Project.Url + $"/{projectId}")
        {
        }
    }
}
