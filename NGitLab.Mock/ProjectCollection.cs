using System;
using System.Linq;
using System.Net;

namespace NGitLab.Mock;

public sealed class ProjectCollection : Collection<Project>
{
    public ProjectCollection(Group group)
        : base(group)
    {
    }

    public Project AddNew()
    {
        return AddNew(configure: null);
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
        else if (this.Any(p => p.Id == project.Id))
        {
            // Cannot do this in GitLab
            throw new NotSupportedException("Duplicate project id");
        }

        if (project.Name == null && project.Path == null)
        {
            throw new GitLabException("Missing name and path")
            {
                // actual GitLab error
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = """name is missing, path is missing""",
            };
        }

        // Auto-generate the Path or Name...
        project.Path ??= Slug.Create(project.Name);
        project.Name ??= project.Path;

        // Name is case sensitive
        if (this.Any(p =>
            p.Path.Equals(project.Path, StringComparison.OrdinalIgnoreCase) ||
            p.Name.Equals(project.Name, StringComparison.Ordinal)))
        {
            throw new GitLabException("Name or Path already exists")
            {
                // actual GitLab error
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = "[\"has already been taken\"]",
            };
        }

        project.RunnersToken ??= Server.GetNewRegistrationToken();

        base.Add(project);
    }
}
