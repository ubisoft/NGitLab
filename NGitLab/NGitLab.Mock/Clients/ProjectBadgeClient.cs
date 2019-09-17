using System;
using System.Collections.Generic;
using NGitLab.Models;

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

        public Badge this[int id] => throw new NotImplementedException();

        public IEnumerable<Badge> All => throw new NotImplementedException();

        public IEnumerable<Badge> ProjectsOnly => throw new NotImplementedException();

        public Badge Create(BadgeCreate badge)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Badge Update(int id, BadgeUpdate badge)
        {
            throw new NotImplementedException();
        }
    }
}
