using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class GroupCollection : Collection<Group>
{
    public GroupCollection(GitLabObject container)
        : base(container)
    {
    }

    public override void Add(Group group)
    {
        if (group is null)
            throw new ArgumentNullException(nameof(group));

        if (group.Id == default)
        {
            group.Id = Server.GetNewGroupId();
        }
        else if (this.Any(g => StringComparer.OrdinalIgnoreCase.Equals(g.Id, group.Id)))
        {
            // Cannot do this in GitLab
            throw new NotSupportedException("Duplicate group id");
        }

        if (group.Name == null && group.Path == null)
        {
            throw new GitLabException("Missing name and path")
            {
                // actual GitLab error
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = """name is missing, path is missing""",
            };
        }

        // Auto-generate the Path or Name...
        group.Path ??= Slug.Create(group.Name);
        group.Name ??= group.Path;

        // Check for conflicts.
        // Mimics GitLab behavior...
        if (this.Any(g => g.Parent == group.Parent && StringComparer.OrdinalIgnoreCase.Equals(g.Path, group.Path)))
        {
            throw new GitLabException("Duplicate group path")
            {
                // actual GitLab error
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = """Failed to save group {:path=>["has already been taken"]}""",
            };
        }

        group.RunnersToken ??= Server.GetNewRegistrationToken();

        base.Add(group);
    }
}
