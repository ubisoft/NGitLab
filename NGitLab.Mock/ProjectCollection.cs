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
            return AddNew(null);
        }

        public Project AddNew(Action<Project> configure)
        {
            var project = new Project();
            configure?.Invoke(project);
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
