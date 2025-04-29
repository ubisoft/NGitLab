using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public Models.Label CreateGroupLabel(long groupId, GroupLabelCreate label)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.Edit);
            return group.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
        }
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

    private static Label FindLabel(LabelsCollection collection, string name)
    {
        return collection.FirstOrDefault(x => x.Name.Equals(name, StringComparison.Ordinal));
    }
}
