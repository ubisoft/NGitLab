using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock.Clients
{
    internal sealed class ProjectBadgeClient : ClientBase, IProjectBadgeClient
    {
        private readonly int _projectId;

        public ProjectBadgeClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public Models.Badge this[int id]
        {
            get
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                var badge = project.Badges.GetById(id);
                if (badge == null)
                    throw new GitLabNotFoundException();

                return badge.ToBadgeModel();
            }
        }

        public IEnumerable<Models.Badge> All
        {
            get
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                return project.Badges.Select(badge => badge.ToBadgeModel());
            }
        }

        public IEnumerable<Models.Badge> ProjectsOnly => All.Where(badge => badge.Kind == Models.BadgeKind.Project);

        public Models.Badge Create(Models.BadgeCreate badge)
        {
            EnsureUserIsAuthenticated();

            var createdBadge = GetProject(_projectId, ProjectPermission.Edit).Badges.Add(badge.LinkUrl, badge.ImageUrl);
            return createdBadge.ToBadgeModel();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Models.Badge Update(int id, Models.BadgeUpdate badge)
        {
            throw new NotImplementedException();
        }
    }
}
