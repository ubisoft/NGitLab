using System;

namespace NGitLab.Mock
{
    public sealed class ProjectCollection : Collection<Project>
    {
        public ProjectCollection(Group group)
            : base(group)
        {
        }

        public override void Add(Project project)
        {
            if (project is null)
                throw new ArgumentNullException(nameof(project));

            if (project.Id == default)
            {
                project.Id = Server.GetNewProjectId();
            }

            base.Add(project);
        }
    }
}
