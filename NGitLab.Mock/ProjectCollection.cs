using System;

namespace NGitLab.Mock
{
    public sealed class ProjectCollection : Collection<Project>
    {
        public ProjectCollection(Group group)
            : base(group)
        {
        }

        public Project AddNew()
        {
            var project = new Project();
            Add(project);
            return project;
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
