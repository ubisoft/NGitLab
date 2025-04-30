using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class LabelClient : ClientBase, ILabelClient
{
    public LabelClient(ClientContext context)
        : base(context)
    {
    }

    public Models.Label CreateProjectLabel(long projectId, ProjectLabelCreate label)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            return project.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
        }
    }

    [Obsolete("Use CreateProjectLabel instead")]
    public Models.Label Create(LabelCreate label)
    {
        return CreateProjectLabel(label.Id, new ProjectLabelCreate
        {
            Name = label.Name,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Models.Label CreateGroupLabel(long groupId, GroupLabelCreate label)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.Edit);
            return group.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
        }
    }

    [Obsolete("Use other CreateGroupLabel instead")]
    public Models.Label CreateGroupLabel(LabelCreate label)
    {
        return CreateGroupLabel(label.Id, new GroupLabelCreate
        {
            Name = label.Name,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Models.Label DeleteProjectLabel(long projectId, ProjectLabelDelete label)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var l = FindLabel(project.Labels, label.Name) ?? throw GitLabException.NotFound($"Cannot find label '{label.Name}'");
            project.Labels.Remove(l);
            return l.ToClientLabel();
        }
    }

    [Obsolete("Use DeleteProjectLabel instead")]
    public Models.Label Delete(LabelDelete label)
    {
        return DeleteProjectLabel(label.Id, new ProjectLabelDelete
        {
            Id = label.Id,
            Name = label.Name,
        });
    }

    public Models.Label EditProjectLabel(long projectId, ProjectLabelEdit label)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var l = FindLabel(project.Labels, label.Name) ?? throw GitLabException.NotFound($"Cannot find label '{label.Name}'");

            if (!string.IsNullOrEmpty(label.NewName))
            {
                l.Name = label.NewName;
            }

            if (!string.IsNullOrEmpty(label.Color))
            {
                l.Color = label.Color;
            }

            if (label.Description != null)
            {
                l.Description = label.Description;
            }

            return l.ToClientLabel();
        }
    }

    [Obsolete("Use EditProjectLabel instead")]
    public Models.Label Edit(LabelEdit label)
    {
        return EditProjectLabel(label.Id, new ProjectLabelEdit
        {
            Name = label.Name,
            NewName = label.NewName,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Models.Label EditGroupLabel(long groupId, GroupLabelEdit label)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.Edit);
            var l = FindLabel(group.Labels, label.Name) ?? throw GitLabException.NotFound($"Cannot find label '{label.Name}'");

            if (!string.IsNullOrEmpty(label.NewName))
            {
                l.Name = label.NewName;
            }

            if (!string.IsNullOrEmpty(label.Color))
            {
                l.Color = label.Color;
            }

            if (label.Description != null)
            {
                l.Description = label.Description;
            }

            return l.ToClientLabel();
        }
    }

    [Obsolete("Use other EditGroupLabel instead")]
    public Models.Label EditGroupLabel(LabelEdit label)
    {
        return EditGroupLabel(label.Id, new GroupLabelEdit
        {
            Name = label.Name,
            NewName = label.NewName,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public IEnumerable<Models.Label> ForGroup(long groupId)
    {
        return ForGroup(groupId, query: null);
    }

    public IEnumerable<Models.Label> ForGroup(long groupId, LabelQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.View);
            return group.Labels.Select(x => x.ToClientLabel());
        }
    }

    public IEnumerable<Models.Label> ForProject(long projectId)
    {
        return ForProject(projectId, query: null);
    }

    public IEnumerable<Models.Label> ForProject(long projectId, LabelQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            return project.Labels.Select(x => x.ToClientLabel());
        }
    }

    public Models.Label GetGroupLabel(long groupId, string name)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.View);
            return FindLabel(group.Labels, name)?.ToClientLabel();
        }
    }

    public Models.Label GetProjectLabel(long projectId, string name)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            return FindLabel(project.Labels, name)?.ToClientLabel();
        }
    }

    [Obsolete("Use GetProjectLabel instead")]
    public Models.Label GetLabel(long projectId, string name)
    {
        return GetProjectLabel(projectId, name);
    }

    private static Label FindLabel(LabelsCollection collection, string name)
    {
        return collection.FirstOrDefault(x => x.Name.Equals(name, StringComparison.Ordinal));
    }
}
